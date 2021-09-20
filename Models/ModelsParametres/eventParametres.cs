using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldEvents.API.Models.ModelsParametres;

namespace WorldEvents.API.Models.ModelsParametres
{
    public class eventParametres : QueryStringParametres
    {
        public DateTime eventYearMin { get; set; }
        public DateTime eventYearMax { get; set; } = DateTime.Now;
        public bool ValideYearRange => eventYearMax > eventYearMin;
    }
}
