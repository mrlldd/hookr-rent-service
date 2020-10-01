using System;
using System.Threading.Tasks;
using Polly;
using Polly.Timeout;

namespace HookrTelegramBot.Utilities.Resiliency
{
    public class PolicySet : IPolicySet
    {
        public IAsyncPolicy ReadPolicy => Policy
            .WrapAsync(ReadTimeoutPolicy,
                ReadRetryPolicy);

        public IAsyncPolicy WritePolicy => WriteTimeoutPolicy;

        private static IAsyncPolicy ReadTimeoutPolicy => Policy
            .TimeoutAsync(30);

        private static IAsyncPolicy ReadRetryPolicy => Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, x => TimeSpan.FromSeconds(Math.Pow(x, 2) / 2));

        private static IAsyncPolicy WriteTimeoutPolicy => Policy
            .TimeoutAsync(50);
    }
}