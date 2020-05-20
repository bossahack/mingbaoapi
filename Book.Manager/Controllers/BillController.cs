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
    public class BillController : ApiController
    {
        public Page<BillShopModel> Search(BillShopParam para)
        {
            return ShopMonthOrderService.GetInstance().Search(para);
        }

        public void Pay(BillPayParam param)
        {
            ShopMonthOrderService.GetInstance().Pay(param);
        }
    }
}
