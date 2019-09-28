using Book.Model;
using Book.Service;
using System.Collections.Generic;
using System.Web.Http;

namespace Book.Api.Controllers
{
    public class ShopController : ApiController
    {
        [Filters.UserFilter]
        [HttpGet]
        public ShopResponse GetInfo(int id)
        {
            return ShopService.GetInstance().GetShopInfo(id);
        }

        [Filters.UserFilter]
        [HttpGet]
        public List<ShopResponse> GetUserShops()
        {
            return ShopService.GetInstance().GetUserShops();
        }

        [Filters.UserFilter]
        [HttpPost]
        public void CollectShop(int shopid)
        {
            ShopService.GetInstance().CollectShop(shopid);
        }
    }
}
