using Book.Dal;
using Book.Utils;
using System;

namespace Book.Service
{
    public class FoodTypeService
    {
        private static FoodTypeService _Instance;
        public static FoodTypeService GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new FoodTypeService();
            return _Instance;
        }
        private static FoodTypeDal foodTypeDal = FoodTypeDal.GetInstance();

        public void Add(string name)
        {
            var currentUser = UserUtil.CurrentUser();
            if (foodTypeDal.ExistName(currentUser.ShopId, name))
                throw new Exception("已存在相同的类型，请检查");

            foodTypeDal.Add(new Dal.Model.FoodType()
            {
                Name = name,
                ShopId = currentUser.ShopId
            });

        }

        public void Edit(int id,string name)
        {
            var currentUser = UserUtil.CurrentUser();
            if (foodTypeDal.ExistName(currentUser.ShopId, name))
                throw new Exception("已存在相同的类型，请检查");

            var foodtype = foodTypeDal.Get(id);
            if (foodtype.ShopId != currentUser.ShopId)
                throw new Exception("您无权限进行此操作");

            foodTypeDal.Edit(id, name);
        }

        public void Remove(int id)
        {
            var currentUser = UserUtil.CurrentUser();

            var foodtype = foodTypeDal.Get(id);
            if (foodtype.ShopId != currentUser.ShopId)
                throw new Exception("您无权限进行此操作");
            if (FoodDal.GetInstance().existType(id))
                throw new Exception("此类型下存在商品，不可删除");

            foodTypeDal.remove(id);
        }
    }
}
