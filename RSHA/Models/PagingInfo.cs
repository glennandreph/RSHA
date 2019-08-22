using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSHA.Models
{
    public class PagingInfo
    {
        public int TotalRequests { get; set; }

        public int RequestsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int totalPage => (int)Math.Ceiling((decimal)TotalRequests / RequestsPerPage);

        public string urlParam { get; set; }
    }
}
