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
}
