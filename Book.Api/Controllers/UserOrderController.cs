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

        [HttpPost]
        public string BookOrder(BookOrderRequest request)
        {
            return orderService.BookOrder(request);
        }

        [HttpPost]
        public void CopyBookOrder(int orderId)
        {
            orderService.CopyBookOrder(orderId);
        }

        [HttpPost]
        public void Cancel(int orderId)
        {
            orderService.CancelByUser(orderId);
        }

        public object GetPages(int index,int size)
        {
            return orderService.GetPages(index, size);
        }

        public object GetOrderDetail(int id)
        {
            return orderService.GetOrderDetail(id);
        }
        
    }
}
