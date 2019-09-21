using EcwidIntegration.Worker.Interfaces;
using System;

namespace EcwidIntegration.Worker.Services
{
    public sealed class ConsoleWriter : IWriter
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }
}
