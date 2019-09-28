using Book.Service;
using System.Web.Http;

namespace Book.Api.Controllers
{
    [Filters.ShopFilter]
    public class SetController : ApiController
    {

        [HttpPost]
        public void Logo(string logo)
        {
            SetService.GetInstance().SetLogo(logo);
        }

        [HttpPost]
        public void ShopName(string shopName)
        {
            SetService.GetInstance().SetShopName(shopName);
        }

        [HttpPost]
        public void ShopAddress(string address)
        {
            SetService.GetInstance().SetShopName(address);
        }

        [HttpPost]
        public void ShopPhone(string phone)
        {
            SetService.GetInstance().SetPhone(phone);
        }

        [HttpPost]
        public void ShopRecommender(string recommender)
        {
            SetService.GetInstance().SetRecommender(recommender);
        }
        
    }
}
