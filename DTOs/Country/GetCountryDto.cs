using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldEvents.API.DTOs.Continent;

namespace WorldEvents.API.DTOs.Country
{
    public class GetCountryDto
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public GetContinentDto continent { get; set; }
    }
}
