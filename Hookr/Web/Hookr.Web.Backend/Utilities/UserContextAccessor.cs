using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Core.Utilities.Caches;
using Hookr.Web.Backend.Utilities.Caches.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hookr.Web.Backend.Utilities
{
    public class UserContextAccessor : IUserContextAccessor
    {
        [NotNull] public Session? Context { get; private set; }

        public void SetContext(Session session)
        {
            Context = session;
        }
    }
}