using Book.Model;
using Book.Service;
using System.Web.Http;

namespace Book.Api.Controllers
{
    public class FoodController : ApiController
    {

        private static FoodTypeService foodtypeService = FoodTypeService.GetInstance();
        private static FoodService foodService = FoodService.GetInstance();

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

        [Filters.UserFilter]
        [HttpGet]
        public object GetList(int type)
        {
            return foodService.GetList(type);
        }

    }
}
