using Book.Dal;
using Book.Dal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Service
{
    public class DictService
    {
        private static DictService _Instance;
        public static DictService GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new DictService();
            return _Instance;
        }

        public Dict Get(string flag)
        {
            return DictDal.GetInstance().Get(flag);
        }

        public List<Dict> GetList(string flags)
        {
            return DictDal.GetInstance().GetList(flags.Split(',').ToList());
        }
    }
}
