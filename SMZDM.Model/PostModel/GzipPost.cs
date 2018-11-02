using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMZDM.Model
{
    public class GzipPost
    {
        /// <summary>
        /// 内容
        /// </summary>
        [JsonProperty("gzipContent")]
        public string GzipContent { get; set; }
    }
}
