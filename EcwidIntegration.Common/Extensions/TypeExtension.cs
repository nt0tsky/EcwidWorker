using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcwidIntegration.Common.Attributes;

namespace EcwidIntegration.Common.Extensions
{
    public static class TypeExtension
    {
        public static bool IsCommon(this Type type)
        {
            return type.GetCustomAttributes(true).Any(t => t.GetType() == typeof(CommonAttribute));
        }
    }
}
