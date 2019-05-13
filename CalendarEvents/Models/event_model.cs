using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Web.Script.Serialization;

namespace WebApplication6.Models
{
    public class events_model : event_base
    {
        public events_model()
        {
            page_size = int.Parse(ConfigurationManager.AppSettings["PageSize"]);
        }

        public int page_size { get; set; }

        public int total { get; set; }

        public int page { get; set; }

        public List<event_model> items { get; set; }
    }

    public class event_model : event_base
    {
        public Guid id { get; set; }

        [Required, Display(Name = "Title")]
        public string title { get; set; }

        [Required, Display(Name = "Description")]
        public string description { get; set; }

        [Required, Display(Name = "Start Date")]
        public string startDate { get; set; }

        [Required, Display(Name = "End Date")]
        public string endDate { get; set; }

        [JsonIgnore]
        public string eventDate
        {
            get { return $"{DateTime.Parse(startDate).ToString("g")} to {DateTime.Parse(endDate).ToString("g")}"; }
        }
    }
}