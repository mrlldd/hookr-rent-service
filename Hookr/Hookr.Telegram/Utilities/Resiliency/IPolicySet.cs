using Polly;

namespace Hookr.Telegram.Utilities.Resiliency
{
    public interface IPolicySet
    {
        IAsyncPolicy ReadPolicy { get; }
        IAsyncPolicy WritePolicy { get; }
    }
}