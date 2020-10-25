using System;
using System.Threading.Tasks;
using Hookr.Core.Repository.Context.Entities;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Core.Utilities.Loaders;
using Hookr.Core.Utilities.Providers;
using Hookr.Web.Backend.Operations.Base;
using Hookr.Web.Backend.Utilities;

namespace Hookr.Web.Backend.Operations.Commands.Auth
{
    public class GenerateRefreshTokenCommandHandler : CommandHandler<GenerateRefreshTokenCommand>
    {
        private const int RefreshExpirationDays = 14;
        private readonly IUserContextAccessor userContextAccessor;

        public GenerateRefreshTokenCommandHandler(IUserContextAccessor userContextAccessor)
        {
            this.userContextAccessor = userContextAccessor;
        }

        public override Task ExecuteCommandAsync(GenerateRefreshTokenCommand command)
        {
            userContextAccessor
                .Modify(x => x.RefreshTokens
                    .Add(RefreshTokenFactory())
                );
            return Task.CompletedTask;
        }

        private static RefreshToken RefreshTokenFactory()
            => new RefreshToken
            {
                ExpiresAt = DateTime.UtcNow.AddDays(RefreshExpirationDays),
                Value = Guid.NewGuid(),
                Used = false
            };
    }
}