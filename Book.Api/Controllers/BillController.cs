using Book.Api.Filters;
using Book.Model;
using Book.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Book.Api.Controllers
{
    [ShopFilter]
    public class BillController : ApiController
    {
        public List<BillModel> GetLast()
        {
            return ShopMonthOrderService.GetInstance().GetLast();
        }

        ///// <summary>
        ///// 回调接口
        ///// </summary>
        ///// <param name="billIds"></param>
        ///// <param name="fee"></param>
        //public void Finish(int id,decimal fee)
        //{
        //    ShopMonthOrderService.GetInstance().Pay(id,fee);
        //}

        public void ZeroPay(int id)
        {
            ShopMonthOrderService.GetInstance().ZeroPay(id);
        }

        /// <summary>
        /// 统一下单接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string unifiedOrder(int id)
        {
            return ShopMonthOrderService.GetInstance().UnifiedOrder(id);
        }

        public string PayCallBack()
        {
            System.IO.Stream  stream = System.Web.HttpContext.Current.Request.InputStream;

            return ShopMonthOrderService.GetInstance().PayCallBack(stream);

        }
    }
}
