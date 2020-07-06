using System;
using System.Threading.Tasks;
using Polly;

namespace HookrTelegramBot.Utilities.Resiliency
{
    public interface IPolicySet
    {
        IAsyncPolicy ReadPolicy { get; }
        IAsyncPolicy WritePolicy { get; }
    }
}