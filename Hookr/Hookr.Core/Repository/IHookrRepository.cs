using System;
using System.Threading;
using System.Threading.Tasks;
using Hookr.Core.Repository.Context;

namespace Hookr.Core.Repository
{
    public interface IHookrRepository : IRepository<HookrContext>
    {
    }
}