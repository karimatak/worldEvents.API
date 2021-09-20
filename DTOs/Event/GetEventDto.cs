using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldEvents.API.DTOs.Category;
using WorldEvents.API.DTOs.Country;

namespace WorldEvents.API.DTOs.Event
{
    public class GetEventDto
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventDetails { get; set; }
        public DateTime? EventDate { get; set; }
        public GetCountryDto Country { get; set; }
        public GetCategoryDto Category { get; set; }
    }
}
