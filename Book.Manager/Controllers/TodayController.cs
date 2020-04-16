using Book.Manager.Filters;
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
    public class TodayController : ApiController
    {
        public object GetHot()
        {
            return new
            {
                RegisterNum = UserService.GetInstance().GetRegisterNum(),
                RegisterNumByUser = UserService.GetInstance().GetRegisterNumByUser(),
                RegisterNumByShop = UserService.GetInstance().GetRegisterNumByShop(),
                RegisterShopNum = ShopService.GetInstance().GetRegisterShopNum(),
                ShopOrderNum=ShopDayOrderService.GetInstance().GetShopOrderNum()
            };
        }
    }
}
