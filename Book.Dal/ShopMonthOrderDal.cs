﻿using Book.Dal.Model;
using Book.Model.Enums;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Dal
{
    public class ShopMonthOrderDal
    {
        private static ShopMonthOrderDal _Instance;
        public static ShopMonthOrderDal GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new ShopMonthOrderDal();
            return _Instance;
        }

        public void Init(DateTime begin,DateTime end,int year,int month)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Execute(@"INSERT into shop_month_order(shop_id,year,month,qty,effect_qty,should_pay,status)
SELECT id,@year,@month,0,0,0,0 from shop where create_date>=@begin and create_date<@end and not EXISTS(select 1 FROM shop_month_order mo where mo.shop_id=shop.id and `month`=@month and year=@year)", new { begin = begin.ToString("yyyy-MM-dd"),end=end.ToString("yyyy-MM-dd"),year,month });
            }
        }

        public void Calc(DateTime createBegin, DateTime createEnd,DateTime orderBegin,DateTime orderEnd, int year, int month)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Execute(@"
UPDATE shop_month_order monthorder,(SELECT SUM( dayorder.effect_qty) effectqty,dayorder.shop_id from shop_day_order dayorder where  dayorder.date>=@orderBegin and dayorder.date<=@orderEnd GROUP BY dayorder.shop_id ) b,(SELECT SUM( dayorder.qty) qty,dayorder.shop_id from shop_day_order dayorder where  dayorder.date>=@orderBegin and dayorder.date<=@orderEnd GROUP BY dayorder.shop_id ) a
set monthorder.effect_qty=b.effectqty,monthorder.qty=a.qty,should_pay=b.effectqty/10,status=@status,generate_date=now()
WHERE year=@year and `month`=@month and monthorder.shop_id in (SELECT id FROM shop where create_date >=@createBegin and create_date<@createEnd) and monthorder.shop_id=b.shop_id and monthorder.shop_id=a.shop_id", new { createBegin = createBegin.ToString("yyyy-MM-dd"), createEnd = createEnd.ToString("yyyy-MM-dd"), orderBegin=orderBegin.ToString("yyyy-MM-dd"), orderEnd= orderEnd.ToString("yyyy-MM-dd"), year, month,status=(int)BillStatus.UnPay });
            }

        }

        public List<ShopMonthOrder> GetByStatus(int shopId, int status)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.Query<ShopMonthOrder>(@"SELECT * from shop_month_order WHERE shop_id=@shopId and `status` =@status", new { shopId = shopId, status = status }).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<ShopMonthOrder> GetTopList(int shopId, int count)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.Query<ShopMonthOrder>(@"SELECT * from shop_month_order WHERE shop_id=@shopId LIMIT @count", new { shopId = shopId, count = count }).ToList();
            }
        }

        public List<ShopMonthOrder> GetList(List<int> ids)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.Query<ShopMonthOrder>("SELECT * from shop_month_order WHERE id in @ids", new { ids }).ToList();
            }
        }
        public ShopMonthOrder Get(int id)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.QueryFirstOrDefault<ShopMonthOrder>("SELECT * from shop_month_order WHERE id = @id", new { id });
            }
        }

        public void Update(List<ShopMonthOrder> orders)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                orders.ForEach(order=> {
                    conn.Update(order);
                });
            }
        }

        public void Update(ShopMonthOrder order)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Update(order);
            }
        }

        public List<ShopMonthOrder> GetPayedList(DateTime dt)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.Query<ShopMonthOrder>("SELECT * from shop_month_order where `status`=@status and DATEDIFF(@dt,generate_date)=0", new { dt=dt.ToString("yyyy-MM-dd"),status=(int)BillStatus.Payed }).ToList();
            }
        }
    }
}
