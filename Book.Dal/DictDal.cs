using Book.Dal.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Dal
{
    public class DictDal
    {
        private static DictDal _Instance;
        public static DictDal GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new DictDal();
            return _Instance;
        }

        public Dict Get(string flag)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.QueryFirstOrDefault<Dict>("SELECT * from dict where flag=@flag", new { flag = flag });
            }
        }

        public List<Dict> GetList(List<string> flags)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.Query<Dict>("SELECT * from dict where flag in @flags", new { flags = flags }).ToList();
            }
        }
    }
}
