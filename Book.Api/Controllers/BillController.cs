using Book.Api.Filters;
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
    [ShopFilter]
    public class BillController : ApiController
    {
        public List<BillModel> GetLast()
        {
            return ShopMonthOrderService.GetInstance().GetLast();
        }

        /// <summary>
        /// 回调接口
        /// </summary>
        /// <param name="billIds"></param>
        /// <param name="fee"></param>
        public void Finish(List<int> billIds,decimal fee)
        {
            ShopMonthOrderService.GetInstance().Finish(billIds,fee);
        }

        public void ZeroPay(int id)
        {
            ShopMonthOrderService.GetInstance().ZeroPay(id);
        }
    }
}
