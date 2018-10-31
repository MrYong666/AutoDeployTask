using Newtonsoft.Json;
using SMZDM.Common;
using SMZDM.Model;
using SyncVersion.Result;
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
                // var url111 = "http://172.18.9.211/api/GZip/AddPersonList   ";
                //var url = "http://10.200.10.40:8083/event/sync/log_test/v1?dept=clt_qd&iscap=true";
                var url = "http://10.200.10.40:8083/event/sync/log_test/v2?dept=clt_qd&iscap=true";
                var persons = BuildModel();
                result = await GzipHelper(url, persons);
                // result.Wait();
                // var versionResult = HttpClientTool.HttpPost(url, persons);
            }
            catch (Exception ex)
            {
                result = ex.Message;

            }
            return new string[] { "value1", "value2", result };
        }
        public List<Person> BuildModel()
        {
            List<Person> persons = new List<Person>();
            Person person = new Person();
            person.Name = "zhangsan";
            person.Gender = "11";
            persons.Add(person);
            Person person1 = new Person();
            person1.Name = "lisi";
            person1.Gender = "11";
            persons.Add(person1);
            return persons;
        }
        public async Task<string> GzipHelper(string url, List<Person> person)
        {
            string result = string.Empty;
            using (var handler = new HttpClientHandler())
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (var client = new HttpClient(handler, false))
                {
                    string json = JsonConvert.SerializeObject(person);
                    byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
                    // byte[] jsonBytes = Encoding.GetEncoding("iso-8859-1").GetBytes(json);
                    var ms = new MemoryStream();
                    using (var gzip = new GZipStream(ms, CompressionMode.Compress, true))
                    {
                        gzip.Write(jsonBytes, 0, jsonBytes.Length);
                    }
                    ms.Position = 0;
                    var content = new StreamContent(ms);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    content.Headers.ContentEncoding.Add("gzip");
                    var response = await client.PostAsync(url, content);
                    var result11 = await response.Content.ReadAsAsync<GzipResult>();
                }
            }
            return result;
        }
    }
}