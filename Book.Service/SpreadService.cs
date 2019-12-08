using Book.Dal;
using Book.Model;
using Book.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Service
{
    public class SpreadService
    {
        private static SpreadService _Instance;
        public static SpreadService GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new SpreadService();
            return _Instance;
        }

        public List<ShopResponse> GetMyShop()
        {
            var current = UserUtil.CurrentUser();
            //var users = UserInfoDal.GetInstance().GetByRecommender(current.Id);
            //if (users == null || users.Count == 0)
            //    return null;
            var shops = ShopDal.GetInstance().GetListByRecommender(current.Id);
            if (shops == null || shops.Count == 0)
                return null;
            return shops.Select(c => new ShopResponse() {
                Name=c.Name,
                Address=c.Address,
                Id=c.Id,
                CreateDate=c.CreateDate
            }).ToList();
        }

        public List<ShopDayOrderSpreadModel> GetMyShopOrder()
        {
            var current = UserUtil.CurrentUser();
            //var users = UserInfoDal.GetInstance().GetByRecommender(current.Id);
            //if (users == null || users.Count == 0)
            //    return null;
            var shops = ShopDal.GetInstance().GetListByRecommender(current.Id);
            if (shops == null || shops.Count == 0)
                return null;

            var dayOrders = ShopDayOrderDal.GetInstance().getList(shops.Select(c => c.Id).ToList(), DateTime.Now.AddDays(-1));
            if (dayOrders == null || dayOrders.Count == 0)
                return null;

            var result = new List<ShopDayOrderSpreadModel>();
            foreach (var dayOrder in dayOrders)
            {
                var shop = shops.FirstOrDefault(c => c.Id == dayOrder.ShopId);
                result.Add(new ShopDayOrderSpreadModel()
                {
                    Id=dayOrder.Id,
                    ShopId=dayOrder.ShopId,
                    Qty=dayOrder.Qty,
                    EffectQty=dayOrder.EffectQty,
                    ShopName=shop.Name
                });
            }
            return result;
        }

        public List<UserInfoRecommendModel> GetMyRecommenders()
        {
            var current = UserUtil.CurrentUser();
            var users = UserInfoDal.GetInstance().GetByRecommender(current.Id);
            if (users == null || users.Count == 0)
                return null;

            return users.Select(c => new UserInfoRecommendModel()
            {
                Id = c.Id,
                WXName = c.WxName,
                HasShop = c.HasShop
            }).ToList();
        }
    }
}
