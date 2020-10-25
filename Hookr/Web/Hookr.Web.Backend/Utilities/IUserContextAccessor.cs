using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Web.Backend.Utilities.Caches.Sessions;

namespace Hookr.Web.Backend.Utilities
{
    public interface IUserContextAccessor
    {
        [NotNull]Session? Context { get; }
        void SetContext(Session session);
    }
}