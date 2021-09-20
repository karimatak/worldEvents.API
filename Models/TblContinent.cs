using System;
using System.Collections.Generic;

namespace WorldEvents.API.Models
{
    public partial class TblContinent
    {
        public TblContinent()
        {
            TblCountry = new HashSet<TblCountry>();
        }

        public int ContinentId { get; set; }
        public string ContinentName { get; set; }
        public string Summary { get; set; }
        public string FilePath { get; set; }

        public virtual ICollection<TblCountry> TblCountry { get; set; }
    }
}
