using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldEvents.API.DTOs.Event
{
    public class AddEventDto
    {
        public string EventName { get; set; }
        public string EventDetails { get; set; }
        public DateTime? EventDate { get; set; }
        public int? CountryId { get; set; }
        public int? CategoryId { get; set; }
    }
}
