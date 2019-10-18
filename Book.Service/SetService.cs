using Book.Dal;
using Book.Model.Enums;
using Book.Utils;
using System;

namespace Book.Service
{
    public class SetService
    {
        private static SetService _Instance;
        public static SetService GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new SetService();
            return _Instance;
        }

        public void SetLogo(string logo)
        {
            var currentUser = UserUtil.CurrentUser();
            var shop = ShopDal.GetInstance().Get(currentUser.ShopId);
            if (shop.UserId != currentUser.Id)
                throw new Exception("操作失败");

            ShopDal.GetInstance().SetLogo(shop.Id, logo);
        }

        public void SetShopName(string shopName)
        {
            var currentUser = UserUtil.CurrentUser();
            var shop = ShopDal.GetInstance().Get(currentUser.ShopId);
            if (shop.UserId != currentUser.Id)
                throw new Exception("操作失败");

            ShopDal.GetInstance().SetShopName(shop.Id, shopName);
        }

        public void SetAddress(string address)
        {
            var currentUser = UserUtil.CurrentUser();
            var shop = ShopDal.GetInstance().Get(currentUser.ShopId);
            if (shop.UserId != currentUser.Id)
                throw new Exception("操作失败");

            ShopDal.GetInstance().SetAddress(shop.Id, address);
        }

        public void SetPhone(string phone)
        {
            var currentUser = UserUtil.CurrentUser();
            var shop = ShopDal.GetInstance().Get(currentUser.ShopId);
            if (shop.UserId != currentUser.Id)
                throw new Exception("操作失败");

            ShopDal.GetInstance().SetPhone(shop.Id, phone);
        }

        public void SetRecommender(string recommender)
        {
            var currentUser = UserUtil.CurrentUser();
            var shop = ShopDal.GetInstance().Get(currentUser.ShopId);
            if (shop.UserId != currentUser.Id)
                throw new Exception("操作失败");
            if(!string.IsNullOrEmpty(shop.Recommender))
                ShopDal.GetInstance().SetRecommender(shop.Id, recommender);
        }

        public void SetStatus(bool normal)
        {
            var currentUser = UserUtil.CurrentUser();
            var shop = ShopDal.GetInstance().Get(currentUser.ShopId);
            if (shop.UserId != currentUser.Id)
                throw new Exception("操作失败");
            if (shop.Status == (int)ShopStatus.Arrears)
                throw new Exception("欠费状态，不可更改");

            ShopDal.GetInstance().SetStatus(shop.Id, normal?(int)ShopStatus.Normal: (int)ShopStatus.Closed);
        }

    }
}
