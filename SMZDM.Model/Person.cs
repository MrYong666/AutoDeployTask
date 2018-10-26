using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMZDM.Model
{

    public class Person
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("age")]
        public string Gender { get; set; }
    }
}
