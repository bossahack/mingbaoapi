using Book.Dal.Model;
using System.Collections.Generic;
using System.Linq;
using Dapper;
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

        public void Add(UserShop usershop)
        {
            using (var conn = SqlHelper.GetInstance())
            {
               conn.Insert<UserShop>(usershop);
            }
        }
    }
}
