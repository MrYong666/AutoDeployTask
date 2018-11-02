using Newtonsoft.Json;
using SMZDM.Common;
using SMZDM.Model;
using SMZDM.Model.Result;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SyncVersion.Controllers
{
    public class PersonController : ApiController
    {
        string result = string.Empty;
        // GET api/<controller>
        public async Task<IEnumerable<string>> Get()
        {
            try
            {
                //var url = "http://172.18.10.28/api/GZip/AddPersonList   ";
                //var url = "http://10.200.10.40:8083/event/sync/log_test/v1?dept=clt_qd&iscap=true";
                // var url = "http://10.200.10.40:8083/event/sync/log_test/v2?dept=clt_qd&iscap=true";
                var url = "http://10.200.10.40:8083/event/sync/log_test/v4?dept=clt_qd&iscap=true";
                var persons = BuildModel();
                string json = JsonConvert.SerializeObject(persons);
                result = GzipHelper(url, json);
            }
            catch (Exception ex)
            {
                result = ex.Message;

            }
            return new string[] { "value1", "value2", result };
        }
        public string GzipHelper(string url, string person)
        {
            string result = string.Empty;
            GzipPost gzipPost = new GzipPost();
            gzipPost.GzipContent = GZipHelper.GZipCompressString(person);
            var gzipResult = HttpClientTool.HttpPost<GzipResult>(url, gzipPost);
            result = JsonConvert.SerializeObject(gzipResult);
            return result;
        }
        [HttpGet]
        public string Stream1(int i)
        {
            var persons = BuildModel();
            string json = JsonConvert.SerializeObject(persons);
            var result = GZipHelper.GZipCompressString(json);
            return result;
        }
        #region 构造参数
        public List<Person> BuildModel()
        {
            List<Person> persons = new List<Person>();
            Person person = new Person();
            person.Name = HttpUtility.UrlEncode("张三");
            person.Gender = "11";
            persons.Add(person);
            Person person1 = new Person();
            person1.Name = "lisi";
            person1.Gender = "11";
            persons.Add(person1);
            return persons;
        }
        #endregion
    }
}