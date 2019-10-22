using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Watch
{
    class Program
    {
        static void Main(string[] args)
        {
            //调用前面的静态方法，将映射关系注册
            Book.Dal.Model.ColumnMapper.SetMapper();
            new OnlineUser().Listen();
        }
    }
}
