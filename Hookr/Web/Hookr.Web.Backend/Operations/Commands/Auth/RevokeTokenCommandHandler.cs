using System;
using System.Linq;
using System.Threading.Tasks;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Web.Backend.Exceptions.Auth;
using Hookr.Web.Backend.Operations.Base;
using Hookr.Web.Backend.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Hookr.Web.Backend.Operations.Commands.Auth
{
    public class RevokeTokenCommandHandler : CommandHandler<RevokeTokenCommand>
    {
        private readonly IHookrRepository hookrRepository;

        public RevokeTokenCommandHandler(IHookrRepository hookrRepository)
        {
            this.hookrRepository = hookrRepository;
        }

        public override async Task ExecuteCommandAsync(RevokeTokenCommand command)
        {
            var now = DateTime.UtcNow;
            var token = await hookrRepository.ReadAsync((context, cancellationToken)
                    => context.RefreshTokens
                        .FirstOrDefaultAsync(x => !x.Used
                                                  && x.ExpiresAt > now
                                                  && x.Value.Equals(command.Token),
                            cancellationToken),
                Token);
            if (token == null)
            {
                throw new RefreshTokenNotFoundException();
            }

            token.Used = true;
        }
    }
}