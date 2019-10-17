using System;
namespace sXb_service.Models
{
    public class SearchFilter
    {
        public Condition[] Conditions { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
    }
}
