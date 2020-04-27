using Book.Dal.Model;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System;

namespace Book.Dal
{
    public class UserShopDal
    {
        private static UserShopDal _Instance;
        public static UserShopDal GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new UserShopDal();
            return _Instance;
        }

        public List<UserShop> GetByUser(int userid)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.Query<UserShop>("SELECT  * FROM user_shop WHERE user_id=@userid ",new { userid=userid}).ToList();
                return result;
            }
        }

        public bool Exist(int userid,int shopid)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.ExecuteScalar("SELECT 1 FROM user_shop where user_id=@userid and shop_id=@shopid", new { userid = userid,shopid=shopid });
                if (result != null)
                    return true;
                return false;
            }
        }

        public UserShop Get(int userid, int shopid)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.QueryFirstOrDefault<UserShop>("SELECT * FROM user_shop where user_id=@userid and shop_id=@shopid", new { userid = userid, shopid = shopid });
                return result;
            }
        }

        public void Add(UserShop usershop)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Insert<UserShop>(usershop);
            }
        }

        public void Update(UserShop usershop)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Update(usershop);
            }
        }

        public List<UserShop> GetPages(int shopId, int index, int size)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.Query<UserShop>($"SELECT * FROM user_shop where shop_id=@shopid ORDER BY create_date DESC LIMIT {index * size},{size}", new { shopid = shopId }).ToList();
                return result;
            }
        }

        public void CalcUserShopOrder(DateTime date)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.Execute($"UPDATE user_shop a INNER JOIN (select shop_id,user_id,count(1) num from b_order where create_date>=@begin and create_date<@end GROUP BY shop_id, user_id) b on a.shop_id = b.shop_id and a.user_id = b.user_id set a.total = a.total + b.num,a.lasted_date=NOW()", new { begin = date.ToString("yyyy-MM-dd"), end = date.AddDays(1).ToString("yyyy-MM-dd") });
            }
        }
    }
}
