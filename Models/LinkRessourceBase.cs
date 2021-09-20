using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldEvents.API.Models
{
    public class LinkRessourceBase
    {
        public LinkRessourceBase()
        {

        }
        public List<Link> Links { get; set; } = new List<Link>();
    }
}
