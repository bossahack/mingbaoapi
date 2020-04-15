using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Model
{
    public class ShopDayOrderSpreadModel
    {
        public int Id { get; set; }

        public int ShopId { get; set; }
        public string ShopName { get; set; }

        public int Qty { get; set; }
        
        public int EffectQty { get; set; }
    }

    public class ShopDayOrderSearchParam : PageSearch
    {

        public DateTime CreateDateBegin { get; set; }
        public DateTime CreateDateEnd { get; set; }

        public int? ShopId { get; set; }
    }

    public class ShopDayOrderSearchModel
    {
        public int Id { get; set; }

        public int ShopId { get; set; }

        public string ShopName { get; set; }

        public int Qty { get; set; }

        public int EffectQty { get; set; }

        public DateTime Date { get; set; }
    }
}
