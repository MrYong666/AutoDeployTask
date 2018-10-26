using SMZDM.Common;
using SMZDM.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SchedulePackage
{
    class Program
    {
        private static string getLatestVersionUrl = ConfigurationManager.AppSettings["GetLatestVersion"];
        private static string packageDownloadUrl = ConfigurationManager.AppSettings["PackageDownloadUrl"];
        private static string uncompressUrl = ConfigurationManager.AppSettings["UncompressUrl"];
        private static string webSiteUrl = ConfigurationManager.AppSettings["WebSiteUrl"];
        private static string versionsNumber = ConfigurationManager.AppSettings["VersionsNumber"];

        static void Main(string[] args)
        {
            #region Test
            int i1, i2;
            ThreadPool.GetMaxThreads(out i1, out i2);
            Console.WriteLine("Max workerThreads :" + i1 + "  completionPortThreads:" + i2);
            ThreadPool.GetMinThreads(out i1, out i2);
            Console.WriteLine("Min workerThreads:" + i1 + "  completionPortThreads:" + i2);
            Console.WriteLine("     CLR Version: {0}  ", Environment.Version);
            #endregion
            try
            {
                //获取下载地址
                var fileInfo = GetFileInfo(getLatestVersionUrl);
                var latestVersionUrl = fileInfo.DownLoadUrl;
                //文件名
                var fileName = latestVersionUrl.Substring(latestVersionUrl.LastIndexOf('/') + 1);
                //下载
                var flag = FileTool.Download(latestVersionUrl, packageDownloadUrl);
                //解压
                FileTool.DeCompressRar(Path.Combine(packageDownloadUrl, fileName), uncompressUrl);
                var webSiteUrls = webSiteUrl.Split('|').ToList();
                foreach (var webSiteUrl in webSiteUrls)
                {
                    //删除bin目录
                    FileTool.DeleteFile(Path.Combine(webSiteUrl, "bin"));
                    //复制
                    FileTool.CopyFolder(Path.Combine(uncompressUrl, fileName.Remove(fileName.LastIndexOf("."))), webSiteUrl);
                    //改名
                    FileTool.ReName(Path.Combine(webSiteUrl, fileName.Remove(fileName.LastIndexOf("."))), "bin");
                }
                SetConfigValue("VersionsNumber", fileName);
            }
            catch (Exception ex)
            {

            }
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
