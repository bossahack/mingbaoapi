using Book.Model;
using Book.Service;
using System.Collections.Generic;
using System.Web.Http;

namespace Book.Api.Controllers
{
    public class ShopController : ApiController
    {
        #region 店家
        [Filters.ShopFilter]
        [HttpGet]
        public ShopResponse GetInfo()
        {
            return ShopService.GetInstance().GetShopInfo();
        }


        [Filters.ShopFilter]
        [HttpGet]
        public object GetQRCode()
        {
            return ShopService.GetInstance().GetQRCode();
        }

        [Filters.ShopFilter]
        [HttpGet]
        public object GetUsers(int index, int size)
        {
            return ShopService.GetInstance().GetUsers(index,size);
        }

        #endregion


        #region user
        [Filters.UserFilter]
        [HttpGet]
        public ShopModel GetInfo_U(int id)
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

        [Filters.UserFilter]
        [HttpPost]
        public void Create(ShopCreateModel shop)
        {
            ShopService.GetInstance().CreateShop(shop.Phone, shop.Pwd);
        }

        #endregion

    }
}
