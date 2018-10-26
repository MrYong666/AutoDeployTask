using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;
using SMZDM.Common;
using SMZDM.Model;
using VersionManagementCenter.BusinessLogic;

namespace VersionManagementCenter.Controllers
{
    public class VersionController : ApiController
    {
        private static string virtualPackageAdress = ConfigurationManager.AppSettings["PackageAdress"];
        private static string packageUrl = System.Web.Hosting.HostingEnvironment.MapPath("~/Package");
        public HttpResponseMessage GetLatestVersion()
        {
#if DEBUG
            virtualPackageAdress = @"http://172.18.9.211/Package/";
            packageUrl = @"E:\SMZDM\VersionManagementCenter\VersionManagementCenter\Package";
#endif
            VersionResult versionResult = new VersionResult();
            try
            {
                OperationDocument fileServies = new OperationDocument(packageUrl);
                var latestVersion = fileServies.GetLatestVersion();
                var fullPackAgeAdress = Path.Combine(virtualPackageAdress, latestVersion);
                versionResult.DownLoadUrl = fullPackAgeAdress;
                versionResult.FullFileName = latestVersion;
                versionResult.VersionNumber = OperationDocument.VersionNumber(latestVersion);
                versionResult.ErrCode = 200;
            }
            catch (Exception ex)
            {
                versionResult.ErrCode = 417;
                versionResult.ErrMsg = ex.Message;
            }
            string result = JsonConvert.SerializeObject(versionResult);
            return JsonTool.Instance.toJson(result);
        }
    }
}