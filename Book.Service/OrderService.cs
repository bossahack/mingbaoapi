using Book.Dal;
using Book.Dal.Model;
using Book.Model;
using Book.Model.Enums;
using Book.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Book.Service
{
    public class OrderService
    {
        private static OrderService _Instance;
        public static OrderService GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new OrderService();
            return _Instance;
        }
        private static OrderDal orderDal = OrderDal.GetInstance();
        private static OrderItemDal orderItemDal = OrderItemDal.GetInstance();
        private static ShopDal shopDal = ShopDal.GetInstance();

        public OrderResponse GetShopOrderToday()
        {
            var currentUser = UserUtil.CurrentUser();
            var orders = orderDal.GetShopOrderList(currentUser.ShopId, DateTime.Now);
            if (orders == null || orders.Count == 0)
                return null;

            var orderItems = orderItemDal.GetList(orders.Select(c => c.Id).ToList());
            OrderResponse result = new OrderResponse() {
                Orders = new List<OrderVM>(),
                OrderItems = new List<OrderItemVM>(),
                Shops = new List<OrderShopModel>()
            };
            var shops = shopDal.GetList(orders.Select(c => c.ShopId).ToList());
            foreach (var order in orders)
            {
                result.Orders.Add(new OrderVM() {
                    Id=order.Id,
                    ShopId=order.ShopId,
                    UserId=order.UserId,
                    CreateDate=order.CreateDate,
                    Status=order.Status,
                    Note=order.Note,
                    TakeCode=order.TakeCode,
                    ArriveTimeType=order.ArriveTimeType
                });
            }
            foreach(var item in orderItems)
            {
                result.OrderItems.Add(new OrderItemVM() {
                    FoodId=item.FoodId,
                    FoodName=item.FoodName,
                    FoodPrice=item.FoodPrice,
                    Qty=item.Qty,
                    OrderId = item.OrderId
                });
            }
            //foreach(var shop in shops)
            //{
            //    result.Shops.Add(new OrderShopModel() {
            //        Address=shop.Address,
            //        Id=shop.Id,
            //        Name=shop.Name
            //    });
            //}
            return result;
        }

        public void Receipt(int orderId)
        {
            var currentUser = UserUtil.CurrentUser();
            var order = orderDal.Get(orderId);
            if (order == null)
                throw new Exception("未查询到订单信息");
            if (order.ShopId != currentUser.ShopId)
                throw new Exception("您无权操作");
            if (order.Status != (int)OrderStatus.Origin)
                throw new Exception("单据不是初始状态，请刷新重试");

            orderDal.SetStatus(orderId, (int)OrderStatus.Receipted);
        }

        public void Taked(int orderId)
        {
            var currentUser = UserUtil.CurrentUser();
            var order = orderDal.Get(orderId);
            if (order == null)
                throw new Exception("未查询到订单信息");
            if (order.ShopId != currentUser.ShopId)
                throw new Exception("您无权操作");
            if (order.Status != (int)OrderStatus.Receipted)
                throw new Exception("单据不是接单状态，请刷新重试");

            orderDal.SetStatus(orderId, (int)OrderStatus.Completed);
        }

        public void Cancel(int orderId)
        {
            var currentUser = UserUtil.CurrentUser();
            var order = orderDal.Get(orderId);
            if (order == null)
                throw new Exception("未查询到订单信息");
            if (order.ShopId != currentUser.ShopId)
                throw new Exception("您无权操作");
            if (new List<int>() { (int)OrderStatus.Completed }.Contains(order.Status))
                throw new Exception("已完成单据不可取消");

            orderDal.SetStatus(orderId, (int)OrderStatus.Canceled);
        }

        public void Abnormal(int orderId)
        {
            var currentUser = UserUtil.CurrentUser();
            var order = orderDal.Get(orderId);
            if (order == null)
                throw new Exception("未查询到订单信息");
            if (order.ShopId != currentUser.ShopId)
                throw new Exception("您无权操作");
            if (new List<int>() { (int)OrderStatus.Completed }.Contains(order.Status))
                throw new Exception("已完成单据不可异常");

            orderDal.SetStatus(orderId, (int)OrderStatus.Abnormaled);
        }


        public OrderResponse GetUserOrderToday()
        {

            var currentUser = UserUtil.CurrentUser();
            var orders = orderDal.GetUserOrderList(currentUser.Id, DateTime.Now);
            if (orders == null || orders.Count == 0)
                return null;

            var orderItems = orderItemDal.GetList(orders.Select(c => c.Id).ToList());
            OrderResponse result = new OrderResponse()
            {
                Orders = new List<OrderVM>(),
                OrderItems = new List<OrderItemVM>(),
                Shops = new List<OrderShopModel>()
            };
            var shops = shopDal.GetList(orders.Select(c => c.ShopId).ToList());
            foreach (var order in orders)
            {
                result.Orders.Add(new OrderVM()
                {
                    Id = order.Id,
                    ShopId = order.ShopId,
                    UserId = order.UserId,
                    CreateDate = order.CreateDate,
                    Status = order.Status,
                    Note = order.Note,
                    TakeCode = order.TakeCode,
                    ArriveTimeType = order.ArriveTimeType
                });
            }
            foreach (var item in orderItems)
            {
                result.OrderItems.Add(new OrderItemVM()
                {
                    FoodId = item.FoodId,
                    FoodName = item.FoodName,
                    FoodPrice = item.FoodPrice,
                    Qty = item.Qty
                });
            }
            foreach (var shop in shops)
            {
                result.Shops.Add(new OrderShopModel()
                {
                    Address = shop.Address,
                    Id = shop.Id,
                    Name = shop.Name
                });
            }
            return result;
        }

        public OrderResponse GetLastedOrders()
        {

            var currentUser = UserUtil.CurrentUser();
            var orders = orderDal.GetLastedOrders(currentUser.Id, 7);
            if (orders == null || orders.Count == 0)
                return null;

            var orderItems = orderItemDal.GetList(orders.Select(c => c.Id).ToList());
            OrderResponse result = new OrderResponse()
            {
                Orders = new List<OrderVM>(),
                OrderItems = new List<OrderItemVM>(),
                Shops = new List<OrderShopModel>()
            };
            var shops = shopDal.GetList(orders.Select(c => c.ShopId).ToList());
            foreach (var order in orders)
            {
                result.Orders.Add(new OrderVM()
                {
                    Id = order.Id,
                    ShopId = order.ShopId,
                    UserId = order.UserId,
                    CreateDate = order.CreateDate,
                    Status = order.Status,
                    Note = order.Note,
                    TakeCode = order.TakeCode,
                    ArriveTimeType = order.ArriveTimeType
                });
            }
            foreach (var item in orderItems)
            {
                result.OrderItems.Add(new OrderItemVM()
                {
                    FoodId = item.FoodId,
                    FoodName = item.FoodName,
                    FoodPrice = item.FoodPrice,
                    Qty = item.Qty
                });
            }
            foreach (var shop in shops)
            {
                result.Shops.Add(new OrderShopModel()
                {
                    Address = shop.Address,
                    Id = shop.Id,
                    Name = shop.Name
                });
            }
            return result;
        }

        public void BookOrder(BookOrderRequest request)
        {
            if (request == null || request.Items == null)
                throw new Exception("异常，请重新下单");
            var currentUser = UserUtil.CurrentUser();
            if (!UserShopDal.GetInstance().Exist(currentUser.Id, request.ShopId))
                throw new Exception("您未关注该商家，不可下单");

            var shop = shopDal.Get(request.ShopId);
            if (shop == null)
                throw new Exception("未找到该商家");
            if (shop.Status == (int)ShopStatus.Closed)
                throw new Exception("商家今日不营业");
            if (shop.Status == (int)ShopStatus.Arrears)
                throw new Exception("商家异常，不可下单");
            var foods = FoodDal.GetInstance().GetList(request.Items.Select(c=>c.FoodId).ToList());
            if (foods.Exists(c => c.Status != (int)FoodStatus.Normal))
                throw new Exception("部分商品已下架，请刷新页面重试");

            var border = new BOrder() {
                UserId=currentUser.Id,
                ShopId=request.ShopId,
                Status=(int)OrderStatus.Origin,
                ArriveTimeType=request.ArriveTimeType,
                Note=request.Note,
                CreateDate=DateTime.Now
            };
            var orderItems = new List<BOrderItem>();
            foreach( var item in request.Items)
            {
                var food = foods.FirstOrDefault(c => c.Id == item.FoodId);
                orderItems.Add(new BOrderItem() {
                    FoodId=item.FoodId,
                    FoodName=food.Name,
                    FoodPrice=food.Price,
                    Qty=item.Qty,
                });
            }
            TransactionHelper.Run(()=> {
                var orderid=orderDal.Create(border);
                orderItems.ForEach(c => c.OrderId = orderid);
                orderItemDal.Create(orderItems);
            });
        }

        public void CopyBookOrder(int orderId)
        {
            var order = orderDal.Get(orderId);
            var currentUser = UserUtil.CurrentUser();

            if (!UserShopDal.GetInstance().Exist(currentUser.Id,order.ShopId))
                throw new Exception("您未关注该商家，不可下单");

            var shop = shopDal.Get(order.ShopId);
            if (shop == null)
                throw new Exception("未找到该商家");
            if (shop.Status == (int)ShopStatus.Closed)
                throw new Exception("商家今日不营业");
            if (shop.Status == (int)ShopStatus.Arrears)
                throw new Exception("商家异常，不可下单");

            var items = orderItemDal.GetList(orderId);
            var foods = FoodDal.GetInstance().GetList(items.Select(c => c.FoodId).ToList());
            if (foods.Exists(c => c.Status != (int)FoodStatus.Normal))
                throw new Exception("部分商品已下架，请刷新页面重试");


            var border = new BOrder()
            {
                UserId = currentUser.Id,
                ShopId = order.ShopId,
                Status = (int)OrderStatus.Origin,
                ArriveTimeType = order.ArriveTimeType,
                Note = order.Note,
                CreateDate = DateTime.Now
            };
            var orderItems = new List<BOrderItem>();
            foreach (var item in items)
            {
                var food = foods.FirstOrDefault(c => c.Id == item.FoodId);
                orderItems.Add(new BOrderItem()
                {
                    FoodId = item.FoodId,
                    FoodName = food.Name,
                    FoodPrice = food.Price,
                    Qty = item.Qty,
                });
            }
            TransactionHelper.Run(() => {
                var orderid = orderDal.Create(border);
                orderItems.ForEach(c => c.OrderId = orderid);
                orderItemDal.Create(orderItems);
            });
        }
    }
}
