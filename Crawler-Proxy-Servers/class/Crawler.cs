using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler_Proxy_Servers.Classes
{
    public class Crawler
    {
        [Key]
        public int Id { get; set; }

        public DateTime DtInicio { get; set; }

        public DateTime DtTermino { get; set; }

        public int QtdPaginas { get; set; }

        public int QtdLinhas { get; set; }
    }
}
