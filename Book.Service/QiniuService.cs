using Book.Dal;
using Book.Utils;
using Qiniu.Http;
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

        public void RemoveUnuseImg()
        {
            bool hasMore = true;
            string marker = string.Empty;
            List<string> UnuseImgs = new List<string>();

            Config config = new Config();
            config.Zone = Zone.ZONE_CN_North;
            Mac mac = new Mac(AccessKey, SecretKey);
            BucketManager bucketManager = new BucketManager(mac, config);

            while (hasMore)
            {
                var listRet = getQiniuSourcesLimit(marker,bucketManager);
                if (listRet.Code != (int)HttpCode.OK)
                    throw new Exception("获取列表失败");

                if (listRet.Result.Items == null || listRet.Result.Items.Count == 0)
                    break;
                foreach(var item in listRet.Result.Items)
                {
                    if(item.Key.StartsWith("source"))
                    {
                        continue;
                    }
                    if (item.Key == "logo.png")
                        continue;
                    if(!VImgAllDal.GetInstance().HasImg(item.Key))
                    {
                        UnuseImgs.Add(item.Key);
                    }
                }

                marker = listRet.Result.Marker;
                if (string.IsNullOrEmpty(listRet.Result.Marker))
                {
                    hasMore = false;
                    break;
                }
            }
            if (UnuseImgs.Count > 0)
            {
                removeImgs(UnuseImgs, bucketManager);
            }

        }

        private ListResult getQiniuSourcesLimit(string marker, BucketManager bucketManager)
        {
            int limit = 50;
           
            ListResult listRet = bucketManager.ListFiles(Bucket, "", marker, limit, "");
            return listRet;
        }

        private void removeImgs(List<string> imgs, BucketManager bucketManager)
        {
            List<string> ops = new List<string>();
            foreach (string key in imgs)
            {
                string op = bucketManager.DeleteOp(Bucket, key);
                ops.Add(op);
            }

            BatchResult ret = bucketManager.Batch(ops);

            if (ret.Code != (int)HttpCode.OK)
            {
                Console.WriteLine("删除出错了");
            }
            foreach (BatchInfo info in ret.Result)
            {
                if (info.Code == (int)HttpCode.OK)
                {
                    Console.WriteLine("删除成功");
                }
                else
                {
                    Console.WriteLine(info.Data.Error);
                }
            }
        }

        
    }
}
