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
    public class UserOrderController : ApiController
    {
        private static OrderService orderService = OrderService.GetInstance();

        public object GetOrderToday()
        {
            return orderService.GetUserOrderToday();
        }

        public object GetLastedOrders()
        {
            return orderService.GetLastedOrders();
        }

        public void BookOrder(BookOrderRequest request)
        {
            orderService.BookOrder(request);
        }

        public void CopyBookOrder(int orderId)
        {
            orderService.CopyBookOrder(orderId);
        }


    }
}
