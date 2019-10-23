using Book.Dal.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Dal
{
    public class ShopOnLineDal
    {
        private static ShopOnLineDal _Instance;
        public static ShopOnLineDal GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new ShopOnLineDal();
            return _Instance;
        }

        public ShopOnline Get(int shopId)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.QueryFirst<ShopOnline>("SELECT * from shop_online where shop_id =@shopid", new { shopid = shopId });
                return result;
            }
        }

        public int Create(ShopOnline shopOnline)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.Insert<ShopOnline>(shopOnline).Value;
            }
        }

        public int Update(ShopOnline shopOnline)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.Execute("update shop_online set ip=@ip,port=@port,last_keep_time=@time where shop_id=@shopid", new { ip= shopOnline.Ip,time= shopOnline.LastKeepTime,shopid= shopOnline.ShopId, port = shopOnline.Port });
            }
        }
    }
}
