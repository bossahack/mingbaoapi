using Book.Dal;
using Book.Model;
using Book.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Service
{
    public class AdminService
    {
        private static AdminService _Instance;
        public static AdminService GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new AdminService();
            return _Instance;
        }
        
        public object  Login(AdminLoginParam para)
        {
            var admin = AdminDal.GetInstance().Get(para.UserName);
            if (admin == null|| admin.Pwd != SecurityUtil.GetInstance().GetMD5String(para.UserPwd))
                throw new Exception("用户名密码错误");
            
            return new
            {
                ID = admin.Id,
                Token = SecurityUtil.GetInstance().EncryptString(admin.Id.ToString())
            };
        }
    }
}
