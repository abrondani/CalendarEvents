using System.Collections.Generic;

namespace CalendarEvents.Models
{
    public class EventBase
    {
        public string message { get; set; }

        public int statusCode { get; set; }

        public List<string> details { get; set; }
    }
}