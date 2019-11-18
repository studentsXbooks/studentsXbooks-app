using System;
using System.Collections.Generic;

namespace sXb_service.Models
{
    public class SearchFilter
    {
        public IEnumerable<Condition> Conditions { get; set; } = new List<Condition>();
        public int? MinPrice { get; set; } = 0;
        public int? MaxPrice { get; set; } = int.MaxValue;
    }
}
