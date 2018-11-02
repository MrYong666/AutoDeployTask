using SMZDM.Common;
using SMZDM.Model;
using SMZDM.Model.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace VersionManagementCenter.Controllers
{
    public class GZipController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpPost]
        // POST api/<controller>
        public GzipResult AddPersonList([FromBody]HttpContent personList)
        {
            GzipResult result = new GzipResult();
            var request = HttpContext.Current.Request;
            request.InputStream.Position = 0;//核心代码
            byte[] byts = new byte[request.InputStream.Length];
            request.InputStream.Read(byts, 0, byts.Length);
            result.msg = GZipHelper.GZipDecompressbyte(byts);
            result.code = "200";
            return result;
        }
    }

}