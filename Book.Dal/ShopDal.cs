using System.Collections.Generic;
using System.Linq;
using Dapper;
using Book.Dal.Model;
using Book.Model.Enums;
using Book.Model;
using System.Text;

namespace Book.Dal
{
    public class ShopDal
    {
        private static ShopDal _Instance;
        public static ShopDal GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new ShopDal();
            return _Instance;
        }

        public Shop Get(int id)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.Get<Shop>(id);
                return result;
            }
        }

        public List<Shop> GetList(List<int> ids)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.Query<Shop>("SELECT * from shop where id in @ids", new { ids = ids }).ToList();
                return result;
            }
        }

        public Shop GetByUser(int userId)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.QueryFirstOrDefault<Shop>("SELECT * from shop where user_id =@userid", new { userid = userId });
                return result;
            }
        }

        public void SetLogo(int shopid, string logo)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Execute("UPDATE shop SET logo=@logo where id=@id", new { logo = logo, id = shopid });
            }
        }

        public void SetShopName(int shopid, string shopName)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Execute("UPDATE shop SET name=@name where id=@id", new { name = shopName, id = shopid });
            }
        }

        public void SetAddress(int shopid, string address)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Execute("UPDATE shop SET address=@address where id=@id", new { address = address, id = shopid });
            }
        }

        public void SetPhone(int shopid, string phone)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Execute("UPDATE shop SET wx_phone=@phone where id=@id", new { phone = phone, id = shopid });
            }
        }

        public void SetRecommender(int shopid, string recommender)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Execute("UPDATE shop SET recommender=@recommender where id=@id", new { recommender = recommender, id = shopid });
            }
        }

        public void SetStatus(int shopid, int status)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Execute("UPDATE shop SET Status=@status where id=@id", new { status = status, id = shopid });
            }
        }


        public List<Shop> GetListByRecommender(int userId)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.Query<Shop>("SELECT * from shop where recommender = @id", new { id = userId }).ToList();
                return result;
            }
        }

        public int Create(Shop shop)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.Insert<Shop>(shop);
                return result.Value;
            }
        }

        public void CloseUnPayShop()
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Execute("UPDATE shop set STATUS=@shopStatus where id in (SELECT shop_id from shop_month_order where `status`=@payStatus AND DATEDIFF(now(),generate_date)>1) and `status`<>@shopStatus", new { payStatus = (int)BillStatus.UnPay, shopStatus = (int)ShopStatus.Arrears });
            }
        }


        public Page<Shop> Search(ShopSearchParam para)
        {
            StringBuilder sb = new StringBuilder($"Where create_date>=@dateBegin and create_date<@dateEnd ");
            var p = new DynamicParameters();
            p.Add("dateBegin", para.CreateDateBegin);
            p.Add("dateEnd", para.CreateDateEnd);
            if (para.Status.HasValue)
            {
                sb.Append("and status=@status ");
                p.Add("status", para.Status);
            }
            
            using (var conn = SqlHelper.GetInstance())
            {
                var where = sb.ToString();
                var total = conn.ExecuteScalar<int>("select  * from shop " + where, p);
                if (total == 0)
                    return new Page<Shop>()
                    {
                        Total = 0,
                        Items = null
                    };
                var items = conn.GetListPaged<Shop>(para.Index, para.Size, where, "create_date desc", p);
                return new Page<Shop>()
                {
                    Total = total,
                    Items = items.ToList()
                };
            }
        }
    }
}
