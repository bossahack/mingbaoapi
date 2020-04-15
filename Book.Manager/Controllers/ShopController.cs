using Book.Manager.Filters;
using Book.Model;
using Book.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Book.Manager.Controllers
{
    [AdminFilter]
    public class ShopController : ApiController
    {
        public Page<ShopSearchModel> Search(ShopSearchParam para)
        {
            return ShopService.GetInstance().Search(para);
        }

        public Page<ShopDayOrderSearchModel> SearchDayOrder(ShopDayOrderSearchParam para)
        {
            return ShopDayOrderService.GetInstance().Search(para);
        }
    }
}
