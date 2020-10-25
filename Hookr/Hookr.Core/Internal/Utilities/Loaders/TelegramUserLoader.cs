using System;
using System.Threading;
using System.Threading.Tasks;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Core.Utilities.Caching;
using Hookr.Core.Utilities.Loaders;
using Microsoft.EntityFrameworkCore;

namespace Hookr.Core.Internal.Utilities.Loaders
{
    internal class TelegramUserLoader : CachingLoader<int, TelegramUser>
    {
        private readonly IHookrRepository hookrRepository;
        private const int MemoryTimeoutMinutes = 10;
        private const int DistributedTimeoutMinutes = 20;
        protected override string CacheKey => "users";

        protected override CachingOptions MemoryCacheOptions { get; } =
            CachingOptions.Enabled(TimeSpan.FromMinutes(MemoryTimeoutMinutes));

        protected override CachingOptions DistributedCacheOptions { get; } =
            CachingOptions.Enabled(TimeSpan.FromMinutes(DistributedTimeoutMinutes));

        public TelegramUserLoader(IHookrRepository hookrRepository)
        {
            this.hookrRepository = hookrRepository;
        }

        protected override Task<TelegramUser> LoadAsync(int args, CancellationToken token = default)
            => hookrRepository
                .ReadAsync((context, cancellationToken) => context.TelegramUsers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == args, token), token);

        protected override string CacheKeySuffixFactory(int args)
            => args.ToString();
    }
}