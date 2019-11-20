using Book.Dal.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Dal
{
    public class ShopDayOrderDal
    {
        private static ShopDayOrderDal _Instance;
        public static ShopDayOrderDal GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new ShopDayOrderDal();
            return _Instance;
        }

        public void Create(ShopDayOrder shopDayOrder)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Insert<ShopDayOrder>(shopDayOrder);
            }
        }

        public ShopDayOrder get(int shopId, DateTime day)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.QueryFirstOrDefault<ShopDayOrder>("SELECT * from shop_day_order where shop_id=@shopid and date=@date", new { shopid = shopId,date=day.ToString("yyyy-MM-dd") });
            }
        }

        public int AddQty(int shopId,int qty,DateTime day)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.Execute("UPDATE shop_day_order set qty=qty+@qty where shop_id=@shopid and date=@date", new { shopid = shopId, date = day.ToString("yyyy-MM-dd"), qty = qty });
                return result;
            }            
        }


        public List<ShopDayOrder> getList(List<int> shopIds, DateTime day)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.Query<ShopDayOrder>("SELECT * from shop_day_order where shop_id in @shopids and date=@date", new { shopIds = shopIds, date = day.ToString("yyyy-MM-dd") }).ToList();
            }
        }

        /// <summary>
        /// n天内的有效单量
        /// </summary>
        /// <param name="fromDate"></param>
        public void CalcShopDayOrder(DateTime fromDate)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Execute(@"update main set main.effect_qty=SELECT count(1) from b_order border where border.shop_id=main.shop_id and border.`status`=20 and border.create_date>=@date and DATEDIFF(border.create_date,main.date)=0
FROM shop_day_order main
where main.date >= @date",new { date= fromDate.ToString("yyyy-MM-dd")});
            }
        }
    }
}
