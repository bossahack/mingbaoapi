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
    /// <summary>
    /// 商家工作台
    /// </summary>

    [Filters.ShopFilter]
    public class OrderController : ApiController
    {
        private static OrderService orderService = OrderService.GetInstance();

        public object GetOrderToday()
        {
            return orderService.GetShopOrderToday();
        }

        public OrderResponse GetShopOrderAfter(DateTime? dt)
        {

            return orderService.GetShopOrderAfter(dt);
        }

        public void Receipt(int orderId)
        {
            orderService.Receipt(orderId);
        }

        public void Taked(int orderId)
        {
            orderService.Taked(orderId);
        }

        public void Cancel(int orderId)
        {
            orderService.CancelByShop(orderId);
        }

        public void Abnormal(int orderId)
        {
            orderService.Abnormal(orderId);
        }

        public ShopOrderHistoryResponse GetPages(int index,int size)
        {
            return orderService.GetShopOrderPages(index, size);
        }

    }
}
