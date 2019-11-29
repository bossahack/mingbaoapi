using Book.Dal.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Dal
{
    public class UserFeeRecordDal
    {

        private static UserFeeRecordDal _Instance;
        public static UserFeeRecordDal GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new UserFeeRecordDal();
            return _Instance;
        }

        public void Create(UserFeeRecord record)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Insert<UserFeeRecord>(record);
            }
        }
    }
}
