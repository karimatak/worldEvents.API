using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldEvents.API.Models;

namespace WorldEvents.API.DTOs.Category
{
    public class GetCategoryDto 
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
