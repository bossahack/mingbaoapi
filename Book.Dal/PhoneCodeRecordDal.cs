using Book.Dal.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Dal
{
    public class PhoneCodeRecordDal
    {
        private static PhoneCodeRecordDal _Instance;
        public static PhoneCodeRecordDal GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new PhoneCodeRecordDal();
            return _Instance;
        }

        public void Create(PhoneCodeRecord record)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                conn.Insert<PhoneCodeRecord>(record);
            }
        }

        public PhoneCodeRecord GetFirst(int userId)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.QueryFirstOrDefault<PhoneCodeRecord>("select  * from phone_code_record where user_id=@0 order by create_time desc",new { userId });
            }
        }

        public int GetTotalAfterTime(int userId,DateTime dt)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.QueryFirstOrDefault<int>("SELECT count(1) from phone_code_record where user_id=@userId and create_time>@dt", new { userId,dt=dt.ToString("yyyy-MM-dd HH:mm:ss") });
            }
        }

    }
}
