using Book.Dal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
namespace Book.Dal
{
    public class OrderDal
    {
        private static OrderDal _Instance;
        public static OrderDal GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new OrderDal();
            return _Instance;
        }

        public List<BOrder> GetShopOrderToday(int shopid)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.Query<BOrder>("SELECT *  FROM b_order WHERE shop_id=@shopid AND create_date>=@today", new { shopid = shopid, today = DateTime.Now.ToString("yyyy-MM-dd") }).ToList();
                return result;
            }
        }

        public List<BOrder> GetShopOrderAfter(int shopid,DateTime dt)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.Query<BOrder>("SELECT *  FROM b_order WHERE shop_id=@shopid AND create_date>=@dt", new { shopid = shopid, dt = dt.ToString("yyyy-MM-dd HH:mm:ss") }).ToList();
                return result;
            }
        }

        public List<BOrder> GetUserOrderList(int userId, DateTime endtime)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.Query<BOrder>("SELECT *  FROM b_order WHERE user_id=@userid AND create_date>=@today", new { userid = userId, today = endtime.ToString("yyyy-MM-dd") }).ToList();
                return result;
            }
        }

        public List<BOrder> GetLastedOrders(int userId, int count)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.Query<BOrder>("SELECT  * FROM b_order WHERE user_id=@userid ORDER BY create_date DESC LIMIT 0,@count", new { userid = userId,@count=count }).ToList();
                return result;
            }
        }

        public BOrder Get(int orderId)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.Get<BOrder>(orderId);
                return result;
            }
        }

        public void SetStatus(int id,int status)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Execute("update b_order set `status`=@status WHERE id=@id", new { id = id, status = status });
            }
        }

        public int Create(BOrder order)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.Insert<BOrder>(order).Value;
            }
        }

        public List<BOrder> GetPages(int userId, int index, int size)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.Query<BOrder>($"SELECT * from b_order WHERE user_id=@userid ORDER BY create_date DESC LIMIT {index * size},{size}", new { userid = userId }).ToList();
                return result;
            }
        }
        public List<BOrder> GetShopOrderPages(int shopId, int index, int size)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.Query<BOrder>($"SELECT * from b_order WHERE shop_id=@userid ORDER BY create_date DESC LIMIT {index * size},{size}", new { userid = shopId }).ToList();
                return result;
            }
        }
    }
}
