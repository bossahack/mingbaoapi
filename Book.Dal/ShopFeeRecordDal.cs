using Book.Dal.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Dal
{
    public class ShopFeeRecordDal
    {

        private static ShopFeeRecordDal _Instance;
        public static ShopFeeRecordDal GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new ShopFeeRecordDal();
            return _Instance;
        }

        public void Create(ShopFeeRecord record)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Insert<ShopFeeRecord>(record);
            }
        }
    }
}
