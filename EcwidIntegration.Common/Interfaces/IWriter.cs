namespace EcwidIntegration.Common.Interfaces
{
    public interface IWriter
    {
        /// <summary>
        /// Записать
        /// </summary>
        /// <param name="message">Сообщение</param>
        void Write(string message);
    }
}
