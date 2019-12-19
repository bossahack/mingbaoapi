using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Book.Service.WxPayApi
{
    public class SafeXmlDocument : XmlDocument
    {
        public SafeXmlDocument()
        {
            this.XmlResolver = null;
        }
    }
}
