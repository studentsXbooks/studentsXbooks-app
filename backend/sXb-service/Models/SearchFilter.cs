using System;
namespace sXb_service.Models
{
    public class SearchFilter
    {
        public Condition[] Conditions { get; set; } = null;
        public int? MinPrice { get; set; } = 0;
        public int? MaxPrice { get; set; } = int.MaxValue;
    }
}
