using Book.Dal.Model;
using Book.Model;
using Book.Model.Enums;
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
        public void CalcShopDayOrderQty(DateTime fromDate)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Execute(@"CREATE TEMPORARY TABLE tmp(
id int,
shop_id int,
date date
);
INSERT into tmp(id,shop_id,date) 
SELECT id,shop_id,date from shop_day_order where date>=@date;

UPDATE shop_day_order a ,(SELECT shop_id,DATE(create_date) date,count(1) qty from b_order
where shop_id in (SELECT shop_id from tmp) and create_date>=@date and status=@status
GROUP BY shop_id,DATE(create_date)) b
set a.effect_qty=b.qty
where a.date=b.date and a.shop_id=b.shop_id;

drop table tmp;", new { date= fromDate.ToString("yyyy-MM-dd"),status=(int)OrderStatus.Completed});
            }
        }


        public Page<ShopDayOrder> Search(ShopDayOrderSearchParam para)
        {
            StringBuilder sb = new StringBuilder($"Where date>=@dateBegin and date<@dateEnd ");
            var p = new DynamicParameters();
            p.Add("dateBegin", para.CreateDateBegin);
            p.Add("dateEnd", para.CreateDateEnd);
            if (para.ShopId.HasValue)
            {
                sb.Append("and shop_id=@shopId ");
                p.Add("shopId", para.ShopId);
            }

            using (var conn = SqlHelper.GetInstance())
            {
                var where = sb.ToString();
                var total = conn.ExecuteScalar<int>("select  count(1) from shop_day_order " + where, p);
                if (total == 0)
                    return new Page<ShopDayOrder>()
                    {
                        Total = 0,
                        Items = null
                    };
                var items = conn.GetListPaged<ShopDayOrder>(para.PageIndex, para.PageSize, where, "date desc", p);
                return new Page<ShopDayOrder>()
                {
                    Total = total,
                    Items = items.ToList()
                };
            }
        }

        public int GetShopOrderNum(DateTime dt)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.ExecuteScalar<int>("SELECT SUM(qty) from shop_day_order where date=@date;", new { date = dt.ToShortDateString() });
                return result;
            }
        }


        public List<ShopDayOrder> GetAfterDay( DateTime day)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.Query<ShopDayOrder>("SELECT * from shop_day_order where  date>=@date", new { date = day.ToString("yyyy-MM-dd") }).ToList();
            }
        }



        public void CalcDayOrderFee(DateTime day)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Execute(@"
UPDATE shop_day_order dayorder
INNER JOIN (SELECT b.shop_id,a.date, SUM(b.total_price) totalprice 
from shop_day_order a
inner join b_order b on a.shop_id=b.shop_id and a.date=DATE(b.create_date)
where a.date>=@dateBegin
and b.total_price is not null
GROUP  BY b.shop_id,a.date) dayorderfee on dayorder.shop_id=dayorderfee.shop_id and dayorder.date=dayorderfee.date
set dayorder.fee=dayorderfee.totalprice
", new { dateBegin = day.ToString("yyyy-MM-dd") });
            }
        }
    }
}
