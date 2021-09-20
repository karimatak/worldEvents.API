using System;
using System.Collections.Generic;

namespace WorldEvents.API.Models
{
    public partial class TblEvent
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventDetails { get; set; }
        public DateTime? EventDate { get; set; }
        public int? CountryId { get; set; }
        public int? CategoryId { get; set; }

        public virtual TblCategory Category { get; set; }
        public virtual TblCountry Country { get; set; }
    }
}
