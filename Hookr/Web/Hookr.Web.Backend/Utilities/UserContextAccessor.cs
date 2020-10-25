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
        private readonly HookrContext hookrContext;
        private readonly ILogger<UserContextAccessor> logger;
        private readonly ICacheProvider cacheProvider;
        [NotNull] private Session? Source { get; set; }
        private Session? Modified { get; set; }
        [NotNull] public Session? Context => Modified ?? Source;

        public UserContextAccessor(HookrContext hookrContext,
            ILogger<UserContextAccessor>logger,
            ICacheProvider cacheProvider)
        {
            this.hookrContext = hookrContext;
            this.logger = logger;
            this.cacheProvider = cacheProvider;
        }

        public void SetContext(Session session)
        {
            Source = session;
        }

        public void Modify(Action<Session> modifier)
        {
            Modified = Clone(Context); 
            modifier(Modified);
        }

        public Task SaveChangesAsync()
            => Modified == null
                ? Task.CompletedTask
                : PerformSavingAsync(Modified);

        private Task PerformSavingAsync(Session session)
        {
            logger
                .LogInformation("Saving context changes");
            hookrContext
                .Entry<TelegramUser>(session).State = EntityState.Modified;
            return cacheProvider
                .UserLevel<Session>()
                .SetAsync(session);
        }
        
        private static Session Clone(Session session)
            => new Session
            {
                Id = session.Id,
                Key = session.Key,
                State = session.State,
                Username = session.Username,
                RefreshTokens = session.RefreshTokens,
                LastUpdatedAt = session.LastUpdatedAt
            }; 
    }
}