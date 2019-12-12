using Book.Dal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Book.Model.Enums;

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
                var result = conn.Query<BOrder>("SELECT  * FROM b_order WHERE user_id=@userid AND create_date<=@dt ORDER BY create_date DESC LIMIT 0,@count", new { userid = userId,@count=count, dt = DateTime.Now.ToString("yyyy-MM-dd") }).ToList();
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

        public void SetStatus(int id,int status,int optUser)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Execute("update b_order set `status`=@status,update_time=@dt,update_user=@optUser WHERE id=@id", new { id = id, status = status,dt=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),optUser=optUser });
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

        /// <summary>
        /// 距离指定时间2天以内的已接单
        /// 距离指定时间2天以内24小时以前的初始状态的单子
        /// </summary>
        public void FinishOrder(DateTime date)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Execute(@"UPDATE b_order set `status`= 20, update_user = -1,update_time=now() where `status`= 10 AND create_date >= DATE_ADD(@date, INTERVAL - 2 DAY);
UPDATE b_order set `status`=20,update_user=-1,update_time=now() where `status`=0 AND create_date>=DATE_ADD(@date,interval -2 DAY) and create_date<date_add(@date, interval -24 hour);
", new { date = date.ToString("yyyy-MM-dd") });
            }
        }

        /// <summary>
        /// 获取用户一段时间内的已完成状态的订单数
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetUserOrderCount(DateTime begin,DateTime end,int userId)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.ExecuteScalar<int>("SELECT COUNT(id) num from b_order where user_id=@userId and create_date BETWEEN @begin and @end and `status`=@status", new {
                    userId=userId,
                    begin = begin,
                    end=end,
                    status=(int)OrderStatus.Completed
                });
            }
        }

        public int GetUserOrderCount(int userId,DateTime dt)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.ExecuteScalar<int>("SELECT COUNT(id) num from b_order where user_id=@userId and create_date > @dt", new
                {
                    userId = userId,
                    dt = dt.ToString("yyyy-MM-dd HH:mm:ss"),
                });
            }
        }
        
    }
}
