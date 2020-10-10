using Polly;

namespace Hookr.Core.Utilities.Resiliency
{
    public interface IPolicySet
    {
        IAsyncPolicy ReadPolicy { get; }
        IAsyncPolicy WritePolicy { get; }
    }
}