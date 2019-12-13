using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Dal
{
    public class VImgAllDal
    {
        private static VImgAllDal _Instance;
        public static VImgAllDal GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new VImgAllDal();
            return _Instance;
        }

        public bool HasImg(string ImgName)
        {
            using (var conn = SqlHelper.GetInstance())
            {
                return conn.ExecuteScalar<int>("SELECT IFNULL((SELECT 1 from v_img_all where img =@img LIMIT 1),0)", new { img = ImgName })==1;
            }
        }
    }
}
