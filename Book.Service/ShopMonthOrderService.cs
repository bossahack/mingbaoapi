using Book.Dal;
using Book.Dal.Model;
using Book.Model;
using Book.Model.Enums;
using Book.Service.WxPayApi;
using Book.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Service
{
    public class ShopMonthOrderService
    {
        private static ShopMonthOrderService _Instance;
        public static ShopMonthOrderService GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new ShopMonthOrderService();
            return _Instance;
        }

        public bool hasUnPay()
        {
            var current = UserUtil.CurrentUser();
            var unpays=ShopMonthOrderDal.GetInstance().GetByStatus(current.ShopId, (int)BillStatus.UnPay);
            if (unpays != null && unpays.Count > 0)
                return true;
            return false;
        }
        public List<BillModel> GetLast()
        {
            var current = UserUtil.CurrentUser();
            var list = ShopMonthOrderDal.GetInstance().GetTopList(current.ShopId, 12);
            if (list == null || list.Count == 0)
                return null;
            var result = new List<BillModel>();
            list.ForEach((item) =>
            {
                result.Add(new BillModel()
                {
                    Id = item.Id,
                    EffectQty = item.EffectQty,
                    Month = item.Month,
                    Qty = item.Qty,
                    ShouldPay = item.ShouldPay,
                    Status = item.Status,
                    Year = item.Year
                });
            });
            return result;

        }

        //public void Pay(int id,decimal fee)
        //{
        //    if (id == 0)
        //        return;
        //    var current = UserUtil.CurrentUser();
        //    var bill = ShopMonthOrderDal.GetInstance().Get(id);
        //    if (bill.Status == (int)BillStatus.Init)
        //    {
        //        throw new Exception("订单不是待付款状态，操作失败");
        //    }
        //    var now = DateTime.Now;
        //    bill.Status = (int)BillStatus.Payed;
        //    bill.PayDate = now;
        //    ShopFeeRecord feeRecord = new ShopFeeRecord() {
        //        CreateTime= now,
        //        Fee=fee,
        //        ShopId=current.ShopId
        //    };
        //    var shop = ShopDal.GetInstance().Get(bill.ShopId);
        //    TransactionHelper.Run(()=> {
        //        ShopMonthOrderDal.GetInstance().Update(bill);
        //        if (shop.Status == (int)ShopStatus.Arrears)
        //        {
        //            ShopDal.GetInstance().SetStatus(shop.Id, (int)ShopStatus.Normal);
        //        }
        //        ShopFeeRecordDal.GetInstance().Create(feeRecord);
        //        //if (shop.Recommender > 0)//job跑
        //        //{
        //        //    UserFeeService.GetInstance().ShopPay(bill, shop.Recommender);
        //        //}
        //    });
        //}

        public void ZeroPay(int id)
        {
            var current = UserUtil.CurrentUser();
            var bill = ShopMonthOrderDal.GetInstance().Get(id);
            if (bill == null)
                throw new Exception("未查到该订账单");
            if (bill.ShopId != current.ShopId)
                throw new Exception("您无权限操作");
            if (bill.ShouldPay > 0)
                throw new Exception("您无权操作");
            if (bill.Status != (int)BillStatus.UnPay)
                throw new Exception("账单不是未付款状态，不可操作");

            var shop = ShopDal.GetInstance().Get(bill.ShopId);

            bill.Status = (int)BillStatus.Payed;
            bill.ShopPayDate = DateTime.Now;

            TransactionHelper.Run(() => {
                ShopMonthOrderDal.GetInstance().Update(bill);
                if (shop.Status == (int)ShopStatus.Arrears)
                {
                    ShopDal.GetInstance().SetStatus(shop.Id, (int)ShopStatus.Normal);
                }
                
            });
        }

        public string UnifiedOrder(int id)
        {
            if (id <= 0)
                throw new Exception("出错了");
            var current = UserUtil.CurrentUser();
            var bill = ShopMonthOrderDal.GetInstance().Get(id);
            if (bill == null)
                throw new Exception("未查到该账单");
            if (bill.Status == (int)BillStatus.Init)
                throw new Exception("账单不是待付款状态，操作失败");
            if (bill.Status == (int)BillStatus.Payed)
                throw new Exception("账单已付款完成，操作失败");
            var now = DateTime.Now;

            WxPayData payData = new WxPayData();
            payData.SetValue("body", "月账单");//商品描述
            payData.SetValue("out_trade_no",bill.TradeNo);//商户订单号 商户系统内部订单号，要求32个字符内，只能是数字、大小写字母_-|*且在同一个商户号下唯一
            payData.SetValue("total_fee", bill.ShouldPay*100);//总金额 单位为分
            payData.SetValue("trade_type", "APP");//支付类型
            payData.SetValue("attach", bill.Id);//附加数据，在查询API和支付通知中原样返回，该字段主要用于商户携带订单的自定义数据

            var result= WxPayApi.WxPayApi.UnifiedOrder(payData, 60);
            var return_code = result.GetValue("return_code").ToString();
            if (return_code.ToUpper() != "SUCCESS")
                throw new Exception(result.GetValue("return_msg").ToString());
            var result_code = result.GetValue("result_code").ToString();
            if (result_code.ToUpper() != "SUCCESS")
                throw new Exception(result.GetValue("err_code_des").ToString());

            var prepay_id = result.GetValue("prepay_id");
            var appVal=  result.MakeSign();
            return appVal;
        }

        public string PayCallBack(System.IO.Stream stream)
        {
            try
            {
                WxPayData payResult = new Notify().GetNotifyData(stream);

                string transaction_id = payResult.GetValue("transaction_id").ToString();

                //查询订单，判断订单真实性
                if (!QueryOrder(transaction_id))
                {
                    //若订单查询失败，则立即返回结果给微信支付后台
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "订单查询失败");
                    Log.Error(this.GetType().ToString(), "Order query failure : " + res.ToXml());
                    return res.ToXml();
                }

                var out_trade_no = payResult.GetValue("out_trade_no").ToString();
                var bill = ShopMonthOrderDal.GetInstance().Get(out_trade_no);
                if (bill == null)
                {
                    Log.Info("", "");
                    return GenerateSuccessResponse();
                }
                if (bill.Status == (int)BillStatus.Payed)
                {
                    Log.Info("", "");
                    return GenerateSuccessResponse();
                }

                bill.Status = (int)BillStatus.Payed;
                bill.ShopPayDate = DateTime.Now;
                var shop = ShopDal.GetInstance().Get(bill.ShopId);
                TransactionHelper.Run(() =>
                {
                    ShopMonthOrderDal.GetInstance().Update(bill);
                    if (shop.Status == (int)ShopStatus.Arrears)
                    {
                        ShopDal.GetInstance().SetStatus(shop.Id, (int)ShopStatus.Normal);
                    }
                });
                return GenerateSuccessResponse();
            }catch (Exception ex)
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "发生意外错误");
                Log.Error(this.GetType().ToString(), ex.ToString());
                return res.ToXml();
            }
        }

        public Page<BillShopModel> Search(BillShopParam para)
        {
            var monthOrders = ShopMonthOrderDal.GetInstance().Search(para);
            if (monthOrders.Total == 0)
                return new Page<BillShopModel>() {
                    Total=0
                };
            var result = new Page<BillShopModel>() {
                Total = monthOrders.Total,
                Items=new List<BillShopModel>()
            };

            var shops = ShopDal.GetInstance().GetList(monthOrders.Items.Select(c => c.ShopId).ToList());
            monthOrders.Items.ForEach(item =>
            {
                var shop = shops.FirstOrDefault(c => c.Id == item.ShopId);
                var bill = new BillShopModel()
                {
                    Id = item.Id,
                    Month = item.Month,
                    Year = item.Year,
                    EffectQty = item.EffectQty,
                    Qty = item.Qty,
                    Status = item.Status,
                    ShopAddress = shop.Address,
                    ShopCreateDate = shop.CreateDate,
                    ShopId = shop.Id,
                    ShopName = shop.Name,
                    ShopStatus = shop.Status,
                    ShouldPay = item.ShouldPay
                };
                result.Items.Add(bill);
            });
            return result;

        }

        private bool QueryOrder(string transaction_id)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res =WxPayApi.WxPayApi.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string GenerateSuccessResponse()
        {
            WxPayData res = new WxPayData();
            res.SetValue("return_code", "SUCCESS");
            res.SetValue("return_msg", "OK");
            Log.Info(this.GetType().ToString(), "order query success : " + res.ToXml());
            return res.ToXml();
        }
    }
}
