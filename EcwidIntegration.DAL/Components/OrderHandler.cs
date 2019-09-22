using System;
using EcwidIntegration.Common.Interfaces;
using EcwidIntegration.DAL.Context;
using EcwidIntegration.DAL.Models;
using Newtonsoft.Json;

namespace EcwidIntegration.DAL.Components
{
    internal class OrderHandler : IEventHandler
    {
        public void Handle(string data)
        {
            using (var context = new ApplicationContext())
            {
                var order = JsonConvert.DeserializeObject<Order>(data);
                if (order != null)
                {
                    context.Orders.Add(order);
                    context.SaveChanges();
                }
            }
        }
    }
}
