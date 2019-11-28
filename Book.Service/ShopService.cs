using Book.Dal;
using Book.Dal.Model;
using Book.Model;
using Book.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;

namespace Book.Service
{
    public class ShopService
    {
        private static ShopService _Instance;
        public static ShopService GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new ShopService();
            return _Instance;
        }


        public ShopResponse GetShopInfo()
        {
            var current = UserUtil.CurrentUser();
            var shop = ShopDal.GetInstance().Get(current.ShopId);
            if (shop == null)
                return null;
            
            return new ShopResponse()
            {
                Id = shop.Id,
                Address = shop.Address,
                Logo = shop.Logo,
                Name = shop.Name,
                Recommender = shop.Recommender,
                Status = shop.Status,
                UnPay= ShopMonthOrderService.GetInstance().hasUnPay()
            };
        }

        public ShopModel GetShopInfo(int id)
        {
            var shop = ShopDal.GetInstance().Get(id);
            if (shop == null)
                return null;
            return new ShopModel()
            {
                Id = shop.Id,
                Address = shop.Address,
                Logo = shop.Logo,
                Name = shop.Name,
                Status = shop.Status
            };
        }

        public List<ShopResponse> GetUserShops()
        {
            var current = UserUtil.CurrentUser();
            var userShopRelations = UserShopDal.GetInstance().GetByUser(current.Id);
            if (userShopRelations == null || userShopRelations.Count == 0)
                return null;

            var shops = ShopDal.GetInstance().GetList(userShopRelations.Select(c=>c.ShopId).ToList());

            var result = new List<ShopResponse>();
            foreach(var shop in shops)
            {
                result.Add(new ShopResponse() {
                    Id = shop.Id,
                    Address = shop.Address,
                    Logo = shop.Logo,
                    Name = shop.Name,
                    Recommender = shop.Recommender,
                    Status = shop.Status
                });
            }
            return result;
        }

        public void CollectShop(int shopid)
        {
            var current = UserUtil.CurrentUser();
            if (UserShopDal.GetInstance().Exist(current.Id, shopid))
                throw new Exception("您已经关注了此商家");

            var usershop = new UserShop()
            {
                UserId = current.Id,
                ShopId = shopid,
                CreateDate = DateTime.Now,
                Total = 0,
            };

            UserShopDal.GetInstance().Add(usershop);
        }

        public object GetQRCode()
        {
            var current = UserUtil.CurrentUser();
            var shop = ShopDal.GetInstance().Get(current.ShopId);
            if (shop == null)
                return null;
            string scene = "id" + current.Id;
            return WxService.GetInstance().GetQrCode(scene);
           
        }

        public void CreateShop(string username, string pwd)
        {
            var current = UserUtil.CurrentUser();
            var userInfo = UserInfoDal.GetInstance().Get(current.Id);
            if (userInfo.HasShop)
                throw new Exception("您已开通过店铺，无需重复开通");
            var userCheck= UserInfoDal.GetInstance().GetByName(username);
            if (userCheck != null)
                throw new Exception("该登录帐号已存在");
            var shop = new Shop()
            {
                UserId = current.Id,
                CreateDate = DateTime.Now,
                Status = (int)Model.Enums.ShopStatus.Normal
            };
            TransactionHelper.Run(()=> {
                userInfo.HasShop = true;
                userInfo.LoginName = username;
                userInfo.LoginPwd = SecurityUtil.GetInstance().GetMD5String(pwd);
                UserInfoDal.GetInstance().Update(userInfo);
                ShopDal.GetInstance().Create(shop);
            });
        }

    }

}
