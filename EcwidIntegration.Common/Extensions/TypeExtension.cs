using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcwidIntegration.Common.Attributes;

namespace EcwidIntegration.Common.Extensions
{
    public static class TypeExtension
    {
        public static bool HasAttributeTypeOf<T>(this Type type)
        {
            return type.GetCustomAttributes(true).Any(t => t.GetType() == typeof(T));
        }
    }
}
