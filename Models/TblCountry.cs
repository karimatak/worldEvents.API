using System;
using System.Collections.Generic;

namespace WorldEvents.API.Models
{
    public partial class TblCountry
    {
        public TblCountry()
        {
            TblEvent = new HashSet<TblEvent>();
        }

        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int? ContinentId { get; set; }

        public virtual TblContinent Continent { get; set; }
        public virtual ICollection<TblEvent> TblEvent { get; set; }
    }
}
