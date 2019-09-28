using System.Collections.Generic;
using System.Linq;
using Dapper;
using Book.Dal.Model;

namespace Book.Dal
{
    public class OrderItemDal
    {
        private static OrderItemDal _Instance;
        public static OrderItemDal GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new OrderItemDal();
            return _Instance;
        }

        public List<BOrderItem> GetList(int orderId)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.Query<BOrderItem>("SELECT * from b_order_item where order_id=@orderid", new { orderid = orderId }).ToList();
                return result;
            }
        }

        public List<BOrderItem> GetList(List<int> orderIds)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                var result = conn.Query<BOrderItem>("SELECT * from b_order_item where order_id in @orderids", new { orderids = orderIds }).ToList();
                return result;
            }
        }

        public void Create(List<BOrderItem> items)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                foreach(var item in items)
                {
                    conn.Insert<BOrderItem>(item);
                }
            }
        }
    }
}
