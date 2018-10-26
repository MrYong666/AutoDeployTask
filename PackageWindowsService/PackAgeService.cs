using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PackageWcfService;

namespace PackageWindowsService
{
    partial class PackAgeService : ServiceBase
    {
        private static object syncRoot = new Object();//同步锁
        private ServiceHost svrHost = null; //寄宿服务对象 
        public PackAgeService()
        {
            InitializeComponent();
        }
        string filePath = @"G:\ServiceLog\ServiceLog.txt";
        protected override void OnStart(string[] args)
        {
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Append))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{DateTime.Now},服务启动前！");
                }
                svrHost = new ServiceHost(typeof(WcfService));
                svrHost.Open();
                using (FileStream stream = new FileStream(filePath, FileMode.Append))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{DateTime.Now},服务启动后！");
                }
            }
            catch (Exception ex)
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Append))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{DateTime.Now};ErrorMsg:{JsonConvert.SerializeObject(ex)},服务启动异常！");
                }
            }

        }

        protected override void OnStop()
        {
            try
            {
                svrHost.Close();
            }
            catch (Exception ex)
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Append))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{DateTime.Now};ErrorMsg:{JsonConvert.SerializeObject(ex)},服务停止异常！");
                }
            }
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
        }
    }
}
