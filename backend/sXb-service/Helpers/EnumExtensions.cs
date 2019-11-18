using System;
using System.Collections.Generic;
using System.Linq;

namespace sXb_service.Helpers
{
    public class EnumNameValue
    {
        public string Name { get; set; }
        public int Value { get; set; }

    }

    public static class EnumExtensions
    {
        public static IEnumerable<EnumNameValue> ToList<T>() where T : Enum
        {
            return EnumExtensions.GetValues<T>()
            .Select((e) => new EnumNameValue() { Name = e.ToString(), Value = (int)Enum.Parse(typeof(T), e.ToString()) });
        }

        public static IEnumerable<T> GetValues<T>() where T : Enum
        {
            return ((T[])Enum.GetValues(typeof(T))).ToList();
        }
    }
}