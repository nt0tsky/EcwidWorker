using System;
using EcwidIntegration.Common.Interfaces;

namespace EcwidIntegration.Common.Services
{
    public sealed class ConsoleWriter : IWriter
    {
        public void Write(string message)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy.MM.dd HH:MM")} - {message}");
        }
    }
}
