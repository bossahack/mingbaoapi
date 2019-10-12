using Qiniu.Storage;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Book.Service
{
    public class QiniuService
    {
        private static string AccessKey = WebConfigurationManager.AppSettings["AccessKey"];
        private static string SecretKey = WebConfigurationManager.AppSettings["SecretKey"];
        private static string Bucket = WebConfigurationManager.AppSettings["Bucket"];
        private static QiniuService _Instance;
        public static QiniuService GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new QiniuService();
            return _Instance;
        }

        public string GetSimpleKey()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket;
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            return token;
        }
    }
}
