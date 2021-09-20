using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldEvents.API.DTOs.Country
{
    public class UpdateCountryDto
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int? ContinentId { get; set; }
    }
}
