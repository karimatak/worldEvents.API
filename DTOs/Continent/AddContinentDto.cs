using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldEvents.API.DTOs.Continent
{
    public class AddContinentDto
    {
        public string ContinentName { get; set; }
        public string Summary { get; set; }
        public IFormFile FilePath { get; set; }
    }
}
