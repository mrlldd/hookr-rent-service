namespace Hookr.Core.Utilities.Providers
{
    public interface IProvider<T>
    {
        void Set(T value);
        T ProvidedValue { get; }
    }
}