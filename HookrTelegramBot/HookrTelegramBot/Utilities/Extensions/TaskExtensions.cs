using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace HookrTelegramBot.Utilities.Extensions
{
    public static class TaskExtensions
    {
        public static Task<(T1, T2)> CombineAsync<T1, T2>(this (Task<T1> First, Task<T2> Second) tasks)
            => tasks.First.ContinueWith(x => tasks.Second.ContinueWith(y => (x.Result, y.Result))).Unwrap();

        public static async Task<IEnumerable<TResult>> ExecuteMultipleAsync<TResult>(this IEnumerable<Func<Task<TResult>>> functors, int maxInParallel = 10)
        {
            var result = new List<TResult>();
            var tasks = new List<Task<TResult>>();
            foreach (var functor in functors)
            {
                tasks.Add(functor());
                if (tasks.Count == maxInParallel)
                {
                    var executedTask = await Task.WhenAny(tasks);
                    result.Add(executedTask.Result);
                    tasks.Remove(executedTask);
                }
            }

            var tasksLeft = await Task.WhenAll(tasks);
            result.AddRange(tasksLeft);
            return result;
        }
    }
}