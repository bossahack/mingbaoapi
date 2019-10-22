using Book.Dal;
using Book.Dal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Service
{
    public class ShopOnlineService
    {
        private static ShopOnlineService _Instance;
        public static ShopOnlineService GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new ShopOnlineService();
            return _Instance;
        }

        public void Save(int shopId,string ip,int point)
        {
            ShopOnline shopOnline = new ShopOnline() {
                Ip=ip,
                ShopId=shopId,
                LastKeepTime= DateTime.Now,
                Point=point.ToString()
            };
            if (ShopOnLineDal.GetInstance().Update(shopOnline) == 0)
            {
                ShopOnLineDal.GetInstance().Create(shopOnline);
            }
        }
    }
}
