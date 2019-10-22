using Book.Dal.Model;
using Dapper;
using System.Collections.Generic;

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

        public List<UserInfo> GetList(List<int> ids)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.QueryFirstOrDefault<List<UserInfo>>("SELECT * FROM user_info WHERE wxid in  @ids", new { ids });
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

    }
}
