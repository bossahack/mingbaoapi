using Book.Dal.Model;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace Book.Dal
{
    public class UserInfoDal
    {
        private static UserInfoDal _Instance;
        public static UserInfoDal GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new UserInfoDal();
            return _Instance;
        }

        public UserInfo Get(string wxid)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.QueryFirstOrDefault<UserInfo>("SELECT * FROM user_info WHERE wxid= @wxid", new { wxid });
                return result;
            }
        }

        public UserInfo Get(int id)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.QueryFirstOrDefault<UserInfo>("SELECT * FROM user_info WHERE id= @id", new { id });
                return result;
            }
        }
        public UserInfo GetByName(string loginName)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.QueryFirstOrDefault<UserInfo>("SELECT * FROM user_info WHERE login_name= @loginName", new { loginName });
                return result;
            }
        }

        public List<UserInfo> GetList(List<int> ids)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.Query<UserInfo>("SELECT * FROM user_info WHERE id in  @ids", new { ids }).ToList();
                return result;
            }
        }

        public int Create(UserInfo userInfo)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.Insert<UserInfo>(userInfo);
                return result.Value;
            }
        }
        public void Update(UserInfo userInfo)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Update<UserInfo>(userInfo);
            }
        }

        public List<UserInfo> GetByRecommender(int recommendId)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.Query<UserInfo>("SELECT * from user_info where recommender=@id", new { id=recommendId }).ToList();
                return result;
            }
        }

    }
}
