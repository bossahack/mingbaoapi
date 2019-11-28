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
    public class BillController : ApiController
    {
        public List<BillModel> GetLast()
        {
            return ShopMonthOrderService.GetInstance().GetLast();
        }
    }
}
