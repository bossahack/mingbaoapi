using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Sms.V20190711;
using TencentCloud.Sms.V20190711.Models;

namespace Book.Service
{
    public class ShortMessageService
    {
        private static string SecretId = "AKID89A1cA4hRuCUVFLtnb7LZWCrFLBkP4pw";// WebConfigurationManager.AppSettings["SmsSecretId"];
        private static string SecretKey = "yfKhndV8eu54nFqFUbS2Yn080d4E7vb4"; //WebConfigurationManager.AppSettings["SmsSecretKey"];
        private static string SmsSdkAppid = "1400419632";// WebConfigurationManager.AppSettings["SmsSdkAppid"];
        private static string SmsSign = "309695";// WebConfigurationManager.AppSettings["SmsSign"]; 
        private static string SmsTemplateID = "708335";// WebConfigurationManager.AppSettings["SmsTemplateID"];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phone">记得前缀:+86</param>
        /// <param name="code"></param>
        public void Send(string phone,string code)
        {
            try
            {
                /* 必要步骤：
                 * 实例化一个认证对象，入参需要传入腾讯云账户密钥对 secretId 和 secretKey
                 * 本示例采用从环境变量读取的方式，需要预先在环境变量中设置这两个值
                 * 您也可以直接在代码中写入密钥对，但需谨防泄露，不要将代码复制、上传或者分享给他人
                 * CAM 密匙查询：https://console.cloud.tencent.com/cam/capi*/
                Credential cred = new Credential
                {
                    SecretId = SecretId,
                    SecretKey = SecretKey
                };
                
                SmsClient client = new SmsClient(cred, "ap-guangzhou");                               
                SendSmsRequest req = new SendSmsRequest();                
                req.SmsSdkAppid = SmsSdkAppid;
                req.Sign = UTF8Encoding.UTF8.GetString(UTF8Encoding.UTF8.GetBytes("我不排队公众号"));
                req.PhoneNumberSet = new String[] {phone };
                req.TemplateID = SmsTemplateID;
                req.TemplateParamSet = new String[] { code };
                SendSmsResponse resp = client.SendSmsSync(req);
                System.Diagnostics.Trace.WriteLine(AbstractModel.ToJsonString(resp));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
