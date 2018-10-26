using SMZDM.Common;
using SMZDM.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace SyncVersion.Controllers
{
    public class SyncVersionController : ApiController
    {
        private static string getLatestVersionUrl = ConfigurationManager.AppSettings["GetLatestVersion"];
        private static string packageDownloadUrl = ConfigurationManager.AppSettings["PackageDownloadUrl"];
        private static string uncompressUrl = ConfigurationManager.AppSettings["UncompressUrl"];
        private static string webSiteUrl = ConfigurationManager.AppSettings["WebSiteUrl"];
        private static string versionsNumber = ConfigurationManager.AppSettings["VersionsNumber"];
        [HttpGet]
        public SyncVersionResult SyncVersion()
        {
            SyncVersionResult syncVersionResult = new SyncVersionResult();
            StringBuilder operationalLog = new StringBuilder();
            try
            {
                //获取下载地址
                var fileInfo = GetFileInfo(getLatestVersionUrl);
                var latestVersionUrl = fileInfo.DownLoadUrl;
                AddLog($"获取最新版本包名称：{fileInfo.FullFileName};", ref operationalLog);
                //文件名
                var fileName = latestVersionUrl.Substring(latestVersionUrl.LastIndexOf('/') + 1);
                //下载
                FileTool.Download(latestVersionUrl, packageDownloadUrl);
                AddLog("Package下载成功;", ref operationalLog);
                //解压
                FileTool.DeCompressRar(Path.Combine(packageDownloadUrl, fileName), uncompressUrl);
                AddLog($"解压到文件夹：{uncompressUrl}", ref operationalLog);
                var webSiteUrls = webSiteUrl.Split('|').ToList();
                foreach (var webSiteUrl in webSiteUrls)
                {
                    //删除bin目录
                    FileTool.DeleteFile(Path.Combine(webSiteUrl, "bin"));
                    AddLog($"移除站点{webSiteUrl}:旧版本Bin目录;", ref operationalLog);
                    //复制
                    FileTool.CopyFolder(Path.Combine(uncompressUrl, fileName.Remove(fileName.LastIndexOf("."))), webSiteUrl);
                    AddLog($"添加站点{webSiteUrl}：新版本Bin目录;", ref operationalLog);
                    //改名
                    FileTool.ReName(Path.Combine(webSiteUrl, fileName.Remove(fileName.LastIndexOf("."))), "bin");
                    AddLog($"站点{webSiteUrl}发布成功!当前最新版本号:{fileInfo.VersionNumber};", ref operationalLog);
                }
                //Todo 记录版本号
                SetConfigValue("VersionsNumber", fileName);
                syncVersionResult.ErrCode = 200;
            }
            catch (Exception ex)
            {
                syncVersionResult.ErrCode = 417;
                syncVersionResult.ErrMsg = ex.Message;
            }
            syncVersionResult.OperationalLog = operationalLog.ToString();
            return syncVersionResult;
        }
        public void AddLog(string content, ref StringBuilder operationalLog)
        {
            operationalLog.AppendLine(content);
        }
        /// <summary>
        /// 修改AppSettings中配置
        /// </summary>
        /// <param name="key">key值</param>
        /// <param name="value">相应值</param>
        public static bool SetConfigValue(string key, string value)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config.AppSettings.Settings[key] != null)
                    config.AppSettings.Settings[key].Value = value;
                else
                    config.AppSettings.Settings.Add(key, value);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static VersionResult GetFileInfo(string getLatestVersionUrl)
        {
            var versionResult = HttpClientTool.HttpGet<VersionResult>(getLatestVersionUrl);
            if (versionResult.ErrCode != 200)
            {
                throw new Exception($"GetLatestVersion Error:{ versionResult.ErrMsg }");
            }
            return versionResult;
        }
    }
}
