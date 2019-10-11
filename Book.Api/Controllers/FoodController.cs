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

        [Filters.UserFilter]
        [HttpGet]
        public object GetList(int type)
        {
            return foodService.GetList(type);
        }

    }
}
