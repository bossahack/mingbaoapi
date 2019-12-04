using Book.Dal.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Dal
{
    public class UserFeeDal
    {
        private static UserFeeDal _Instance;
        public static UserFeeDal GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new UserFeeDal();
            return _Instance;
        }

        public UserFee Get(int userId)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var userfee = conn.QueryFirstOrDefault<UserFee>("select * from user_fee where user_id=@userId", new { userId });
                return userfee;
            }
        }

        public bool Exist(int userId)
        {
            return Get(userId) != null;
        }

        public void Create(UserFee record)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Insert<UserFee>(record);
            }
        }

        public void Plus(int userId, decimal Qty)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.Execute("UPDATE user_fee set total+=@qty where user_id=@userid", new { qty = Qty, userid = userId });
                if (result != 1)
                {
                    throw new Exception("打款失败");
                }
            }
        }

        public void Minus(int userId, decimal Qty)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.Execute("UPDATE user_fee set total-=@qty where user_id=@userid", new { qty = Qty, userid = userId });
                if (result != 1)
                {
                    throw new Exception("扣款失败");
                }
            }
        }
    }
}
