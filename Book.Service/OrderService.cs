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
        private static UserInfoDal userInfoDal = UserInfoDal.GetInstance();

        public OrderResponse GetShopOrderToday()
        {
            var currentUser = UserUtil.CurrentUser();
            var orders = orderDal.GetShopOrderToday(currentUser.ShopId);
            if (orders == null || orders.Count == 0)
                return null;
            orders = orders.OrderByDescending(c => c.CreateDate).ToList();

            var orderItems = orderItemDal.GetList(orders.Select(c => c.Id).ToList());
            var shops = shopDal.GetList(orders.Select(c => c.ShopId).ToList());
            var users = userInfoDal.GetList(orders.Select(c => c.UserId).ToList());
            var abnormals = OrderAbnormalDal.GetInstance().GetList(currentUser.ShopId, users.Select(c => c.Id).ToList(), DateTime.Now.AddDays(-30));
            OrderResponse result = new OrderResponse() {
                Orders = new List<OrderVM>(),
                OrderItems = new List<OrderItemVM>(),
                Users=new List<ShopOrderHistoryUserInfoModel>()
            };
            foreach (var order in orders)
            {
                result.Orders.Add(new OrderVM() {
                    Id=order.Id,
                    //ShopId=order.ShopId,
                    UserId=order.UserId,
                    CreateDate=order.CreateDate,
                    Status=order.Status,
                    Note=order.Note,
                    TakeCode=order.TakeCode,
                    ArriveTimeType=order.ArriveTimeType,
                    HasAbnormal= abnormals.Exists(c=>c.UserId==order.UserId)
                });
            }
            foreach(var item in orderItems)
            {
                result.OrderItems.Add(new OrderItemVM() {
                    Id=item.Id,
                    FoodId=item.FoodId,
                    FoodName=item.FoodName,
                    FoodPrice=item.FoodPrice,
                    Qty=item.Qty,
                    OrderId = item.OrderId
                });
            }
            foreach (var user in users)
            {
                result.Users.Add(new ShopOrderHistoryUserInfoModel()
                {
                    Id=user.Id,
                    WXName=user.WxName,
                    Phone=user.WxPhone
                });
            }
            return result;
        }

        public OrderResponse GetShopOrderAfter(DateTime? dt)
        {
            if (!dt.HasValue)
                dt =DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            var currentUser = UserUtil.CurrentUser();
            var orders = orderDal.GetShopOrderAfter(currentUser.ShopId,dt.Value);
            if (orders == null || orders.Count == 0)
                return null;
            orders = orders.OrderByDescending(c => c.CreateDate).ToList();
            var orderItems = orderItemDal.GetList(orders.Select(c => c.Id).ToList());
            OrderResponse result = new OrderResponse()
            {
                Orders = new List<OrderVM>(),
                OrderItems = new List<OrderItemVM>(),
                Users = new List<ShopOrderHistoryUserInfoModel>()
            };
            var shops = shopDal.GetList(orders.Select(c => c.ShopId).ToList());
            var users = userInfoDal.GetList(orders.Select(c => c.UserId).ToList());
            var abnormals = OrderAbnormalDal.GetInstance().GetList(currentUser.ShopId, users.Select(c => c.Id).ToList(), DateTime.Now.AddDays(-30));
            foreach (var order in orders)
            {
                result.Orders.Add(new OrderVM()
                {
                    Id = order.Id,
                    //ShopId=order.ShopId,
                    UserId = order.UserId,
                    CreateDate = order.CreateDate,
                    Status = order.Status,
                    Note = order.Note,
                    TakeCode = order.TakeCode,
                    ArriveTimeType = order.ArriveTimeType,
                    HasAbnormal = abnormals.Exists(c => c.UserId == order.UserId)
                });
            }
            foreach (var item in orderItems)
            {
                result.OrderItems.Add(new OrderItemVM()
                {
                    Id = item.Id,
                    FoodId = item.FoodId,
                    FoodName = item.FoodName,
                    FoodPrice = item.FoodPrice,
                    Qty = item.Qty,
                    OrderId = item.OrderId
                });
            }
            foreach (var user in users)
            {
                result.Users.Add(new ShopOrderHistoryUserInfoModel()
                {
                    Id = user.Id,
                    WXName = user.WxName,
                    Phone=user.WxPhone
                });
            }
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

            orderDal.SetStatus(orderId, (int)OrderStatus.Receipted,currentUser.Id);
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

            orderDal.SetStatus(orderId, (int)OrderStatus.Completed, currentUser.Id);
        }

        public void CancelByUser(int orderId)
        {
            var currentUser = UserUtil.CurrentUser();
            var order = orderDal.Get(orderId);
            if (order == null)
                throw new Exception("未查询到订单信息");
            if (order.UserId != currentUser.Id)
                throw new Exception("您无权操作");
            if (order.Status!=(int)OrderStatus.Origin)
                throw new Exception("非初始状态订单不可取消");

            orderDal.SetStatus(orderId, (int)OrderStatus.Canceled, currentUser.Id);
            sendUdp(order.ShopId, "c" + order.Id);
        }

        public void CancelByShop(int orderId)
        {
            var currentUser = UserUtil.CurrentUser();
            var order = orderDal.Get(orderId);
            if (order == null)
                throw new Exception("未查询到订单信息");
            if (order.ShopId != currentUser.ShopId)
                throw new Exception("您无权操作");
            if (order.Status != (int)OrderStatus.Abnormaled)
                throw new Exception("非异常状态订单不可取消");
            TransactionHelper.Run(()=> {
                orderDal.SetStatus(orderId, (int)OrderStatus.Canceled, currentUser.Id);
                var abnormal = OrderAbnormalDal.GetInstance().Get(currentUser.ShopId, order.UserId, orderId);
                if (abnormal != null)
                {
                    OrderAbnormalDal.GetInstance().Remove(abnormal.Id);
                }
            });
            sendUdp(order.ShopId, "c" + order.Id);
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
            TransactionHelper.Run(()=> {
                OrderAbnormalDal.Create(new BOrderAbnormal()
                {
                    OrderId = order.Id,
                    ShopId = currentUser.ShopId,
                    UserId = order.UserId,
                    CreateDate = DateTime.Now
                });
                orderDal.SetStatus(orderId, (int)OrderStatus.Abnormaled, currentUser.Id);
            });
        }


        public OrderResponse GetUserOrderToday()
        {

            var currentUser = UserUtil.CurrentUser();
            var orders = orderDal.GetUserOrderList(currentUser.Id, DateTime.Now);
            if (orders == null || orders.Count == 0)
                return null;
            orders = orders.OrderByDescending(c => c.CreateDate).ToList();
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
                    OrderId=item.OrderId,
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

            orders = orders.OrderByDescending(c => c.CreateDate).ToList();
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
                    Id=item.Id,
                    OrderId=item.OrderId,
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

        public string BookOrder(BookOrderRequest request)
        {
            if (request == null || request.Items == null)
                throw new Exception("异常，请重新下单");
            var currentUser = UserUtil.CurrentUser();
            var userShop = UserShopDal.GetInstance().Get(currentUser.Id, request.ShopId);
            if (userShop==null)
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
                return string.Join(",", foods.Where(c => c.Status != (int)FoodStatus.Normal).Select(c => c.Id));

            OrderQtyCheck(currentUser.Id);
            UserAbnormalCheck(currentUser.Id);
            var now = DateTime.Now;
            var border = new BOrder() {
                UserId=currentUser.Id,
                ShopId=request.ShopId,
                Status=(int)OrderStatus.Origin,
                ArriveTimeType=request.ArriveTimeType,
                Note=request.Note,
                CreateDate= now,
                TakeCode= generateTakeCode(request.ShopId, currentUser.Id)
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
            userShop.LastedDate = now;
            TransactionHelper.Run(()=> {
                var orderid=orderDal.Create(border);
                orderItems.ForEach(c => c.OrderId = orderid);
                orderItemDal.Create(orderItems);
                ShopDayOrderService.GetInstance().AddQty(border.ShopId, 1);
                UserShopDal.GetInstance().Update(userShop);
            });
            sendUdp(request.ShopId);
            return null;      
        }

        public void CopyBookOrder(int orderId)
        {
            var order = orderDal.Get(orderId);
            var currentUser = UserUtil.CurrentUser();

            var userShop = UserShopDal.GetInstance().Get(currentUser.Id, order.ShopId);
            if (userShop == null)
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

            OrderQtyCheck(currentUser.Id);
            UserAbnormalCheck(currentUser.Id);
            var now = DateTime.Now;

            var border = new BOrder()
            {
                UserId = currentUser.Id,
                ShopId = order.ShopId,
                Status = (int)OrderStatus.Origin,
                ArriveTimeType = order.ArriveTimeType,
                Note = order.Note,
                CreateDate = now,
                TakeCode = generateTakeCode(order.ShopId,currentUser.Id)
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
            userShop.LastedDate = now;
            TransactionHelper.Run(() => {
                var orderid = orderDal.Create(border);
                orderItems.ForEach(c => c.OrderId = orderid);
                orderItemDal.Create(orderItems);
                ShopDayOrderService.GetInstance().AddQty(border.ShopId, 1);
                UserShopDal.GetInstance().Update(userShop);
            });
            sendUdp(order.ShopId);
        }

        public OrderResponseHistory GetPages(int index, int size)
        {
            var currentUser = UserUtil.CurrentUser();
            var orders = orderDal.GetPages(currentUser.Id, index, size);
            if (orders == null || orders.Count == 0)
                return null;

            orders = orders.OrderByDescending(c => c.CreateDate).ToList();
            var orderItems = orderItemDal.GetList(orders.Select(c => c.Id).ToList());
            OrderResponseHistory result = new OrderResponseHistory()
            {
                Orders = new List<OrderVMHistory>(),
                OrderItems = new List<OrderItemVMHistory>(),
                Shops = new List<ShopModelHistory>()
            };
            var shops = shopDal.GetList(orders.Select(c => c.ShopId).ToList());
            foreach (var order in orders)
            {
                result.Orders.Add(new OrderVMHistory()
                {
                    Id = order.Id,
                    ShopId = order.ShopId,
                    CreateDate = order.CreateDate,
                    Status = order.Status,
                });
            }
            foreach (var item in orderItems)
            {
                result.OrderItems.Add(new OrderItemVMHistory()
                {
                    Id=item.Id,
                    FoodName = item.FoodName,
                    Qty = item.Qty,
                    OrderId = item.OrderId
                });
            }
            foreach(var shop in shops)
            {
                result.Shops.Add(new ShopModelHistory()
                {
                    Id = shop.Id,
                    Name = shop.Name
                });
            }
            return result;
        }

        public ShopOrderHistoryResponse GetShopOrderPages(int index, int size)
        {

            var currentUser = UserUtil.CurrentUser();
            var orders = orderDal.GetShopOrderPages(currentUser.ShopId, index, size);
            if (orders == null || orders.Count == 0)
                return null;
            
            ShopOrderHistoryResponse result = new ShopOrderHistoryResponse()
            {
                Orders = new List<ShopOrderHistory>(),
                OrderItems = new List<ShopOrderItemHistory>(),
                Users=new List<ShopOrderHistoryUserInfoModel>(),
            };
            var shops = shopDal.GetList(orders.Select(c => c.ShopId).ToList());
            var abnormals = OrderAbnormalDal.GetInstance().GetList(currentUser.ShopId, orders.Select(c => c.UserId).ToList(), DateTime.Now.AddDays(-30));
            foreach (var order in orders)
            {
                result.Orders.Add(new ShopOrderHistory()
                {
                    Id = order.Id,
                    CreateDate = order.CreateDate,
                    Status = order.Status,
                    UserId=order.UserId,
                    ArriveTimeType=order.ArriveTimeType,
                    Note=order.Note,
                    TakeCode=order.TakeCode,
                    HasAbnormal= abnormals.Exists(c=>c.UserId==order.UserId)
                });
            }
            var orderItems = orderItemDal.GetList(orders.Select(c => c.Id).ToList());
            foreach (var item in orderItems)
            {
                result.OrderItems.Add(new ShopOrderItemHistory()
                {
                    Id=item.Id,
                    FoodName = item.FoodName,
                    Qty = item.Qty,
                    OrderId = item.OrderId,
                    FoodPrice=item.FoodPrice
                });
            }
            var users = userInfoDal.GetList(orders.Select(c=>c.UserId).Distinct().ToList());
            foreach(var item in users)
            {
                result.Users.Add(new ShopOrderHistoryUserInfoModel()
                {
                    Id = item.Id,
                    WXName = item.WxName,
                    Phone=item.WxPhone
                });
            }
            return result;
        }

        public OrderResponse GetOrderDetail(int orderid)
        {
            var order = orderDal.Get(orderid);
            if (order == null)
                throw new Exception("该订单不存在");
            var currentUser = UserUtil.CurrentUser();

            OrderResponse result = new OrderResponse()
            {
                Orders = new List<OrderVM>(),
                OrderItems = new List<OrderItemVM>(),
                Shops = new List<OrderShopModel>()
            };
            var orderItems = orderItemDal.GetList(order.Id);
            var shop = shopDal.Get(order.ShopId);
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
            foreach (var item in orderItems)
            {
                result.OrderItems.Add(new OrderItemVM()
                {
                    OrderId = item.OrderId,
                    FoodId = item.FoodId,
                    FoodName = item.FoodName,
                    FoodPrice = item.FoodPrice,
                    Qty = item.Qty
                });
            }
            result.Shops.Add(new OrderShopModel()
            {
                Address = shop.Address,
                Id = shop.Id,
                Name = shop.Name
            });
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="msg">a:新单子，c:取消了</param>
        public void sendUdp( int shopId,string msg="n")
        {
            UdpSendHelper.Send(string.Format($"{shopId}&{msg}"));
        }

        private string generateTakeCode(int shopId,int userId)
        {
            int totalQty = 0;
            var dayOrder=ShopDayOrderDal.GetInstance().get(shopId, DateTime.Now);
            if(dayOrder==null)
            {
                totalQty = 1;
            }else
            {
                totalQty = dayOrder.Qty+1;
            }
            return $"{totalQty}-{userId}";
            //var lenght = words.Length;
            //if (totalQty < lenght *10 )
            //{
            //    var l = totalQty / 10;
            //    var r = totalQty % 10;
            //    return $"{words[l]}0{r - 1}";
            //}else
            //{
            //    return "牛"+totalQty.ToString();
            //}
        }

        private void OrderQtyCheck(int userId)
        {
            var qty = OrderDal.GetInstance().GetUserOrderCount(userId, DateTime.Now.AddHours(-3));
            if (qty > 3)
                throw new Exception("3小时内下单不能超过3单");
        }

        private void UserAbnormalCheck(int userId)
        {
            var qty = OrderAbnormalDal.GetInstance().GetUserAbnormalCount(userId, DateTime.Now.AddMonths(-1));
            if(qty>2)
               throw new Exception("一个月内异常单超过2单不可下单");
        }

        //60个字符
        //private static string[] words =new string[] { "A", "B", " C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "喔", "P", "Q", "R", "S", "T", "U", "V", "W", "X","Y","Z", "恭", "喜", "发", "财", "万", "事", "大", "吉", "土", "豪", "啊", "我", "们", "做", "朋", "友", "太", "厉", "嗨", "啦", "哇", "赞", "美", "帅", "绝", "牛", "旺", "顺", "天", "和", "敬", "爽", "鸣", "！" };
    }
}
