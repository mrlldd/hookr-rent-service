using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Hookr.Web.Backend.Utilities.Caches.Sessions;

namespace Hookr.Web.Backend.Utilities
{
    public interface IUserContextAccessor
    {
        [NotNull]Session? Context { get; }
        void SetContext(Session session);

        void Modify(Action<Session> modifier);

        Task SaveChangesAsync();
    }
}