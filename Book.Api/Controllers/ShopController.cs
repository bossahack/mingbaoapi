using Book.Model;
using Book.Service;
using System.Collections.Generic;
using System.Web.Http;

namespace Book.Api.Controllers
{
    public class ShopController : ApiController
    {
        [Filters.ShopFilter]
        [HttpGet]
        public ShopResponse GetInfo()
        {
            return ShopService.GetInstance().GetShopInfo();
        }

        [Filters.ShopFilter]
        [HttpGet]
        public List<ShopResponse> GetUserShops()
        {
            return ShopService.GetInstance().GetUserShops();
        }

        [Filters.ShopFilter]
        [HttpPost]
        public void CollectShop(int shopid)
        {
            ShopService.GetInstance().CollectShop(shopid);
        }


        [Filters.ShopFilter]
        [HttpGet]
        public object GetQRCode()
        {
            return ShopService.GetInstance().GetQRCode();
        }
    }
}
