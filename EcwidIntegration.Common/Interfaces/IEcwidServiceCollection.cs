namespace EcwidIntegration.Common.Interfaces
{
    public interface IEcwidServiceCollection
    {
        T GetService<T>();

        void Init();
    }
}
