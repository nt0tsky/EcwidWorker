using System;
using System.Collections.Generic;

namespace EcwidIntegration.Common.Models
{
    /// <summary>
    /// Элемент для регистрации
    /// </summary>
    public class RegisterItem
    {
        /// <summary>
        /// Интерфейс
        /// </summary>
        public Type Interface { get; set; }

        /// <summary>
        /// Список реализаций
        /// </summary>
        public IList<Type> Implementations { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public RegisterItem()
        {
            Implementations = new List<Type>();
        }
    }
}
