using System;
using System.Collections.Generic;

namespace WorldEvents.API.Models
{
    public partial class VwEvents
    {
        public string Continent { get; set; }
        public string Country { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventDetails { get; set; }
        public DateTime? EventDate { get; set; }
        public string Category { get; set; }
    }
}
