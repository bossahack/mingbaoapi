using Book.Dal.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Dal
{
    public class OrderAbnormalDal
    {
        private static OrderAbnormalDal _Instance;
        public static OrderAbnormalDal GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new OrderAbnormalDal();
            return _Instance;
        }

        public static void Create(BOrderAbnormal abnormal)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Insert<BOrderAbnormal>(abnormal);
            }
        }

        public bool Exist(int shopId, int userId,int orderId)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var sr = conn.ExecuteReader("SELECT 1 from b_order_abnormal where shop_id=@shopId and user_id=@userId and order_id>=@orderid", new { shopId = shopId, userId = userId, orderid = orderId});
                while (sr.Read())
                {
                    return true;
                }
                return false;
            }
        }

        public BOrderAbnormal Get(int shopId, int userId, int orderId)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.QueryFirstOrDefault<BOrderAbnormal>("SELECT * from b_order_abnormal where shop_id=@shopId and user_id=@userId and order_id=@orderid", new { shopId = shopId, userId = userId, orderid = orderId });
            }
        }

        public List<BOrderAbnormal> GetList(int shopId, List<int> users, DateTime afterTime)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.Query<BOrderAbnormal>("SELECT * from b_order_abnormal where shop_id=@shopId and user_id in @users  and create_date>@time", new { shopId = shopId, users = users, time = afterTime.ToString("yyyy-MM-dd") }).ToList();
            }
        }

        public void Remove(int id)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Delete<BOrderAbnormal>(id);
            }

        }
    }
}
