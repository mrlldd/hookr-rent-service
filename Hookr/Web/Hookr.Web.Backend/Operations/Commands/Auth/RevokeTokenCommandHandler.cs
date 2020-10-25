using System.Linq;
using System.Threading.Tasks;
using Hookr.Web.Backend.Exceptions.Auth;
using Hookr.Web.Backend.Operations.Base;
using Hookr.Web.Backend.Utilities;

namespace Hookr.Web.Backend.Operations.Commands.Auth
{
    public class RevokeTokenCommandHandler : CommandHandler<RevokeTokenCommand>
    {
        private readonly IUserContextAccessor userContextAccessor;

        public RevokeTokenCommandHandler(IUserContextAccessor userContextAccessor)
        {
            this.userContextAccessor = userContextAccessor;
        }

        public override Task ExecuteCommandAsync(RevokeTokenCommand command)
        {
            userContextAccessor
                .Modify(session =>
                {
                    var token = session.RefreshTokens
                        .FirstOrDefault(x => x.Value.Equals(command.Token));
                    if (token == null)
                    {
                        throw new RefreshTokenNotFoundException();
                    }

                    token.Used = true;
                });
            return Task.CompletedTask;
        }
    }
}