using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldEvents.API.DTOs.Country;

namespace WorldEvents.API.DTOs.Continent
{
    public class ContinentCountriesDto
    {
        public int ContinentId { get; set; }
        public string ContinentName { get; set; }
        public string Summary { get; set; }
        public List<GetCountriesForContinentDto> Countries { get; set; } = new List<GetCountriesForContinentDto>();
    }
}
