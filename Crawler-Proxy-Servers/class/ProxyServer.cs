using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Crawler_Proxy_Servers.Classes
{
    public class ProxyServer
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        public string IPAdress { get; set; }

        public string Port { get; set; }

        public string Country { get; set; }

        public string Protocol { get; set; }

        [JsonIgnore]
        public int IdCrawler { get; set; }
    }
}
