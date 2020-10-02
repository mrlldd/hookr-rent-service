using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hookr.Telegram.Utilities.Extensions
{
    public static class TaskExtensions
    {
        public static Task<(T1, T2)> CombineAsync<T1, T2>(this (Task<T1> First, Task<T2> Second) tasks)
            => tasks.First.ContinueWith(x => tasks.Second.ContinueWith(y => (x.Result, y.Result))).Unwrap();
        public static async Task<(T1, T2, T3)> CombineAsync<T1, T2, T3>(this (Task<T1> First, Task<T2> Second, Task<T3> Third) tasks)
        {
            await Task.WhenAll(tasks.First, tasks.Second, tasks.Third);
            return (tasks.First.Result, tasks.Second.Result, tasks.Third.Result);
        }

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
        
        public static async Task ExecuteMultipleAsync(this IEnumerable<Func<Task>> functors, int maxInParallel = 10)
        {
            var tasks = new List<Task>();
            foreach (var functor in functors)
            {
                tasks.Add(functor());
                if (tasks.Count == maxInParallel)
                {
                    var executedTask = await Task.WhenAny(tasks);
                    tasks.Remove(executedTask);
                }
            }

            await Task.WhenAll(tasks);
        }
    }
}