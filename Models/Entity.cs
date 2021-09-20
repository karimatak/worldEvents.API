using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace WorldEvents.API.Models
{
    public class Entity 
    {
        private readonly IDictionary<string, object> expando = null;
        public void WriteXml(XmlWriter writer)
        {
            foreach (var key in expando.Keys)
            {
                var value = expando[key];
            }
        }
    }
}
