using Book.Dal.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Dal
{
    public class AdminDal
    {

        private static AdminDal _Instance;
        public static AdminDal GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new AdminDal();
            return _Instance;
        }

        public Admin Get(string name)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.QueryFirstOrDefault<Admin>("SELECT * from admin where name=@name", new { name = name });
            }
        }
    }
}
