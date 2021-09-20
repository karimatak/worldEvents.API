using System;
using System.Collections.Generic;

namespace WorldEvents.API.Models
{
    public partial class TblCategory
    {
        public TblCategory()
        {
            TblEvent = new HashSet<TblEvent>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<TblEvent> TblEvent { get; set; }
    }
}
