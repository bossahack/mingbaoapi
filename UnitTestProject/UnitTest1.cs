using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Book.Dal;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //调用前面的静态方法，将映射关系注册
            Book.Dal.Model.ColumnMapper.SetMapper();
            //new DalUserInfo().Get("aa");
        }
    }
}
