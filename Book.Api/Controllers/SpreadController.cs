using Book.Model;
using Book.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Book.Api.Controllers
{
    [Filters.UserFilter]
    public class SpreadController : ApiController
    {
        public List<ShopResponse> GetMyShop()
        {
            return SpreadService.GetInstance().GetMyShop();
        }
        public List<ShopDayOrderSpreadModel> GetMyShopOrder()
        {
            return SpreadService.GetInstance().GetMyShopOrder();
        }

        public List<UserInfoRecommendModel> GetMyRecommenders(int index)
        {
            return SpreadService.GetInstance().GetMyRecommenders(index,10);
        }
    }
}
