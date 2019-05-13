using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class event_base
    {
        public string message { get; set; }

        public int statusCode { get; set; }

        public List<string> details { get; set; }
    }
}