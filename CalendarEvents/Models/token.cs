using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class token : event_base
    {
        public string access_token { get; set; }

        public DateTime creation_datetime { get; set; }

        public int expires_in { get; set; }
    }
}