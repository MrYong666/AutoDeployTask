using SMZDM.Common;
using SMZDM.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Script.Serialization;

namespace VersionManagementCenter.Ashx
{
    /// <summary>
    /// PublishingSite 的摘要说明
    /// </summary>
    public class PublishingSite : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            StringBuilder resultMsg = new StringBuilder();
            string statusMsg = string.Empty;
            try
            {
                var clientServiceUrl = ConfigurationManager.AppSettings["ClientServiceUrl"];
                var syncVersionMethod = ConfigurationManager.AppSettings["ClientServiceSyncVersionMethod"];
                var clientServiceUrls = clientServiceUrl.Split('|').ToList();
                foreach (var serviceUrl in clientServiceUrls)
                {
                    var syncVersionMethodUrl = clientServiceUrl + syncVersionMethod;
                    var syncVersionResult = SyncVersion(syncVersionMethodUrl);
                    resultMsg.AppendLine(syncVersionResult.OperationalLog);
                }
                statusMsg = "true";
            }
            catch (Exception ex)
            {
                statusMsg = "error";
                resultMsg.Append(ex.Message);
            }
            //Todo发送钉钉消息
            WriteJson(context.Response, statusMsg, resultMsg.ToString());
        }
        public static SyncVersionResult SyncVersion(string url)
        {
            var syncVersionResult = HttpClientTool.HttpGet<SyncVersionResult>(url);
            if (syncVersionResult.ErrCode != 200)
            {
                throw new Exception($"GetLatestVersion Error:{ syncVersionResult.ErrMsg }");
            }
            return syncVersionResult;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public static void WriteJson(HttpResponse response,
        string status1, string msg1, object data1 = null)
        {
            response.ContentType = "application/json";
            var obj = new { status = status1, msg = msg1, data = data1 };
            string json = new JavaScriptSerializer().Serialize(obj);
            response.Write(json);
        }
    }
}