using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldEvents.API.Models
{
    public class LinkCollectionWrapper<T> : LinkRessourceBase
    {
        public List<T> Value { get; set; } = new List<T>();
        public LinkCollectionWrapper()
        {

        }
        public LinkCollectionWrapper(List<T> value)
        {
            Value = value;
        }
    }
}
