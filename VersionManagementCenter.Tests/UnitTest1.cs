using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VersionManagementCenter.Controllers;

namespace VersionManagementCenter.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //var filename = "project-2.rar";
            //var delRar = filename.Remove(filename.LastIndexOf("."));
            //var fileName1 = delRar.Substring(delRar.LastIndexOf('-') + 1);
            VersionController version = new VersionController();
            version.GetLatestVersion();
        }
    }
}
 