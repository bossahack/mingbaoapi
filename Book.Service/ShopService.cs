﻿using Book.Dal;
using Book.Dal.Model;
using Book.Model;
using Book.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

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


        public ShopResponse GetShopInfo(int shopid)
        {
            var shop = ShopDal.GetInstance().Get(shopid);
            if (shop == null)
                return null;
            return new ShopResponse()
            {
                Id = shop.Id,
                Address = shop.Address,
                Logo = shop.Logo,
                Name = shop.Name,
                Recommender = shop.Recommender,
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
    }
}
