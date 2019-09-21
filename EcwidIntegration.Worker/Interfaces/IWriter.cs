namespace EcwidIntegration.Worker.Interfaces
{
    internal interface IWriter
    {
        /// <summary>
        /// Записать
        /// </summary>
        /// <param name="message">Сообщение</param>
        void Write(string message);
    }
}
