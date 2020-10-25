using System.Threading.Tasks;
using Hookr.Web.Backend.Operations.Base;

namespace Hookr.Web.Backend.Operations.Commands.Auth
{
    public class GenerateRefreshTokenCommandHandler : CommandHandler<GenerateRefreshTokenCommand>
    {
        public override Task ExecuteCommandAsync(GenerateRefreshTokenCommand command)
        {
            throw new System.NotImplementedException();
        }
    }
}