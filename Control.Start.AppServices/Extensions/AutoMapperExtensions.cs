using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.AppServices.Extensions
{
    internal static class AutoMapperExtensions
    {
        public static T MapTo<T>(this object value)
        {
            return AutoMapper.Mapper.Map<T>(value);
        }

        public static IEnumerable<T> EnumerableTo<T>(this object value)
        {
            return AutoMapper.Mapper.Map<IEnumerable<T>>(value);
        }
    }
}
