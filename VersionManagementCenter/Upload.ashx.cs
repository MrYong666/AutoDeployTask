using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace VersionManagementCenter
{
    /// <summary>
    /// upload 的摘要说明
    /// </summary>
    public class Upload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string resultMsg = string.Empty;
            string statusMsg = string.Empty;
            try
            {
                if (context.Request.Files.Count > 0)
                {
                    var packageFile = ConfigurationManager.AppSettings["PackageFile"];
                    HttpPostedFile file1 = context.Request.Files["myfile"];
                    uploadFile(file1, $"{packageFile}/");  //这里引用的是上面封装的方法
                    statusMsg = "true";
                }
                else
                {
                    WriteJson(context.Response, "error", "请选择要上传的文件");
                }
            }
            catch (Exception ex)
            {
                statusMsg = "error";
                resultMsg = ex.Message;
            }
            WriteJson(context.Response, statusMsg, resultMsg);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="file">通过form表达提交的文件</param>
        /// <param name="virpath">文件要保存的虚拟路径</param>
        public static void uploadImg(HttpPostedFile file, string virpath)
        {
            if (file.ContentLength > 1024 * 1024 * 4)
            {
                throw new Exception("文件不能大于4M");
            }
            string imgtype = Path.GetExtension(file.FileName);
            if (imgtype != ".jpg" && imgtype != ".jpeg")  //图片类型进行限制
            {
                throw new Exception("请上传jpg或JPEG图片");
            }
            using (Image img = Bitmap.FromStream(file.InputStream))
            {
                string savepath = HttpContext.Current.Server.MapPath(virpath + file.FileName);
                img.Save(savepath);
            }
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">通过form表达提交的文件</param>
        /// <param name="virpath">文件要保存的虚拟路径</param>
        public static void uploadFile(HttpPostedFile file, string virpath)
        {
            if (file.ContentLength > 1024 * 1024 * 10)
            {
                throw new Exception("文件不能大于6M");
            }
            string imgtype = Path.GetExtension(file.FileName);
            //imgtype对上传的文件进行限制
            if (imgtype != ".zip" && imgtype != ".mp3" && imgtype != ".rar")
            {
                throw new Exception("只允许上传zip、rar....文件");
            }
            string dirFullPath = HttpContext.Current.Server.MapPath(virpath);
            if (!Directory.Exists(dirFullPath))//如果文件夹不存在，则先创建文件夹
            {
                Directory.CreateDirectory(dirFullPath);
            }
            file.SaveAs(dirFullPath + file.FileName);
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