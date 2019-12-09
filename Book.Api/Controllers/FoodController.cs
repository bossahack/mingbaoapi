using Book.Dal.Model;
using Book.Model;
using Book.Service;
using System.Collections.Generic;
using System.Web.Http;

namespace Book.Api.Controllers
{
    public class FoodController : ApiController
    {

        private static FoodTypeService foodtypeService = FoodTypeService.GetInstance();
        private static FoodService foodService = FoodService.GetInstance();


        #region 用户端

        [Filters.UserFilter]
        public List<FoodTypeResponse> GetTypes_U(int id)
        {
            return foodtypeService.GetShopTypes(id);
        }



        [Filters.UserFilter]
        [HttpGet]
        public object GetList_U(int shopid)
        {
            return foodService.GetShopFoods(shopid);
        }
        #endregion

        #region shop
        [Filters.ShopFilter]
        public List<FoodTypeResponse> GetTypes()
        {
            return foodtypeService.GetTypes();
        }

        [Filters.ShopFilter]
        [HttpPost]
        public void AddType(string name)
        {
            foodtypeService.Add(name);
        }

        [Filters.ShopFilter]
        [HttpPost]
        public void EditType(int id, string name)
        {
            foodtypeService.Edit(id, name);
        }

        [Filters.ShopFilter]
        [HttpPost]
        public void RemoveType(int id)
        {
            foodtypeService.Remove(id);
        }



        [Filters.ShopFilter]
        [HttpPost]
        public void Create(FoodRequest request)
        {
            if (string.IsNullOrEmpty(request.Img))
            {
                request.Img = "https://bpic.588ku.com/art_water_pic/19/07/13/4ab51bea856b0993ec8c9eb220b3ed7d.jpg";
            }
            foodService.Create(request);
        }

        [Filters.ShopFilter]
        [HttpPost]
        public void Edit(FoodRequest request)
        {
            foodService.Edit(request);
        }

        [Filters.ShopFilter]
        [HttpPost]
        public void SetEnable(int id)
        {
            foodService.setEnable(id);
        }

        [Filters.ShopFilter]
        [HttpPost]
        public void SetDisable(int id)
        {
            foodService.SetDisable(id);
        }

        [Filters.ShopFilter]
        [HttpPost]
        public void SetRemoved(int id)
        {
            foodService.SetRemoved(id);
        }

        [Filters.ShopFilter]
        [HttpPost]
        public void SetFoodImg([FromUri]int id, [FromUri]string path)
        {
            foodService.SetFoodImg(id,path);
        }


        [Filters.ShopFilter]
        [HttpPost]
        public void TypeReorder([FromBody]List<FoodTypeResponse> types)
        {
            foodtypeService.TypeReorder(types);
        }

        [Filters.ShopFilter]
        [HttpPost]
        public void FoodReorder([FromBody]List<Food> foods)
        {
            foodService.FoodReorder(foods);
        }

        [Filters.ShopFilter]
        [HttpGet]
        public object GetList(int type)
        {
            return foodService.GetList(type);
        }
        #endregion
    }
}
