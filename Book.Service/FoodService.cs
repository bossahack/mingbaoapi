using Book.Dal;
using Book.Dal.Model;
using Book.Model;
using Book.Model.Enums;
using Book.Utils;
using System;
using System.Collections.Generic;

namespace Book.Service
{
    public class FoodService
    {

        private static FoodService _Instance;
        public static FoodService GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new FoodService();
            return _Instance;
        }
        private static FoodDal foodDal = FoodDal.GetInstance();

        public void Create(FoodRequest request)
        {
            var currentUser = UserUtil.CurrentUser();
            Food food = new Food() {
                CreateDate=DateTime.Now,
                Img=request.Img,
                Intro=request.Intro,
                Name=request.Name,
                Price=request.Price,
                ShopId= currentUser.ShopId,
                Status=(int)FoodStatus.Normal,
                Type=request.Type
            };
            foodDal.Add(food);
        }

        public void Edit(FoodRequest request)
        {
            if (!request.Id.HasValue)
                throw new Exception("未知食食品，请确认");
            var foodDb = foodDal.Get(request.Id.Value);
            if (foodDb == null)
                throw new Exception("未查询到该食品");

            var currentUser = UserUtil.CurrentUser();
            if (foodDb.ShopId != currentUser.ShopId)
                throw new Exception("您没有权限进行此操作");

            foodDb.Img = request.Img;
            foodDb.Intro = request.Intro;
            foodDb.Name = request.Name;
            foodDb.Price = request.Price;
            foodDb.Status = request.Status;
            foodDb.Type = request.Type;
            foodDal.edit(foodDb);
        }

        public void setEnable(int id)
        {
            var food = foodDal.Get(id);
            if (food == null)
                throw new Exception("未查询到该食品");

            var currentUser = UserUtil.CurrentUser();
            if (food.ShopId != currentUser.ShopId)
                throw new Exception("您没有权限进行此操作");

            foodDal.enable(id);
        }

        public void SetDisable(int id)
        {
            var food = foodDal.Get(id);
            if (food == null)
                throw new Exception("未查询到该食品");

            var currentUser = UserUtil.CurrentUser();
            if (food.ShopId != currentUser.ShopId)
                throw new Exception("您没有权限进行此操作");

            foodDal.disable(id);
        }

        public List<Food> GetList(int type)
        {
            var currentUser = UserUtil.CurrentUser();
            return foodDal.GetList(type, currentUser.ShopId);
        }
    }
}
