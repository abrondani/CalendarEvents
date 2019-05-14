using System;

namespace CalendarEvents.Models
{
    public class Token : EventBase
    {
        public string access_token { get; set; }

        public int expires_in { get; set; }
    }
}