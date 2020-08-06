using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace HookrTelegramBot.Utilities.Extensions
{
    public static class TaskExtensions
    {
        public static Task<(T1, T2)> CombineAsync<T1, T2>(this (Task<T1> First, Task<T2> Second) tasks)
            => tasks.First.ContinueWith(x => tasks.Second.ContinueWith(y => (x.Result, y.Result))).Unwrap();
    }
}