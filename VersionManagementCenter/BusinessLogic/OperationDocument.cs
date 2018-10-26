using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VersionManagementCenter.BusinessLogic
{
    public class OperationDocument
    {
        public static List<string> packageList = new List<string>();
        public OperationDocument(string packageUrl)
        {
            DirectoryInfo folder = new DirectoryInfo(packageUrl);
            foreach (FileInfo file in folder.GetFiles())
            {
                packageList.Add(file.Name);
            }
        }
        public string GetLatestVersion()
        {
            var maxPackageName = packageList.OrderByDescending(i =>
            {
                //var delPostfixFileName = i.Remove(i.LastIndexOf("."));
                //var version = delPostfixFileName.Substring(delPostfixFileName.LastIndexOf('-') + 1);
                //return version;
                return VersionNumber(i);
            }).FirstOrDefault();
            return maxPackageName;
        }
        public static string VersionNumber(string fullFileName)
        {
            var delPostfixFileName = fullFileName.Remove(fullFileName.LastIndexOf("."));
            var version = delPostfixFileName.Substring(delPostfixFileName.LastIndexOf('-') + 1);
            return version;
        }
    }
}
