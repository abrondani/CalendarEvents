using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace CalendarEvents.Models
{
    public class EventsModel : EventBase
    {
        public EventsModel()
        {
            page_size = int.Parse(ConfigurationManager.AppSettings["PageSize"]);
        }

        public int page_size { get; set; }

        public int total { get; set; }

        public int page { get; set; }

        public List<EventModel> items { get; set; }
    }

    public class EventModel : EventBase
    {
        public Guid id { get; set; }

        [Required(ErrorMessage = "Title is required"), Display(Name = "Title")]
        public string title { get; set; }

        [Required(ErrorMessage = "Description is required"), Display(Name = "Description")]
        public string description { get; set; }

        [Required(ErrorMessage = "Start Date is required"), Display(Name = "Start Date")]
        public string startDate { get; set; }

        [Required(ErrorMessage = "End Date is required"), Display(Name = "End Date")]
        public string endDate { get; set; }

        [JsonIgnore]
        public string eventDate
        {
            get { return $"{DateTime.Parse(startDate).ToString("g")} to {DateTime.Parse(endDate).ToString("g")}"; }
        }
    }
}