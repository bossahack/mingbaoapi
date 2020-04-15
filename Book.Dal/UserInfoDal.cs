using Book.Dal.Model;
using Book.Model;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public List<UserInfo> GetByRecommender(int recommendId, int index, int size)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.Query<UserInfo>($"SELECT * from user_info where recommender=@id ORDER BY create_date DESC LIMIT {index * size},{size}", new { id=recommendId }).ToList();
                return result;
            }
        }

        public Page<UserInfo> Search(UserSearchParam para)
        {
            StringBuilder sb = new StringBuilder($"Where create_date>=@dateBegin and create_date<@dateEnd ");
            var  p = new DynamicParameters();
            p.Add("dateBegin",para.CreateDateBegin);
            p.Add("dateEnd",para.CreateDateEnd);
            if (para.HasShop.HasValue)
            {
                sb.Append("and has_shop=@hasShop ");
                p.Add("hasShop",para.HasShop);
            }
            if(para.IsRecommender.HasValue)
            {
                if(para.IsRecommender.Value)
                    sb.Append("and type=1 ");
                else
                    sb.Append("and(type is null or type<>1) ");                
            }
            using (var conn = SqlHelper.GetInstance())
            {
                var where = sb.ToString();
                var total = conn.ExecuteScalar<int>("select  * from user_info " + where, p);
                if (total == 0)
                    return new Page<UserInfo>() {
                        Total=0,
                        Items=null
                    };
                var items = conn.GetListPaged<UserInfo>(para.PageIndex, para.PageSize, where,"create_date desc", p);
                return new Page<UserInfo>()
                {
                    Total = total,
                    Items = items.ToList()
                };
            }
        }

    }
}
