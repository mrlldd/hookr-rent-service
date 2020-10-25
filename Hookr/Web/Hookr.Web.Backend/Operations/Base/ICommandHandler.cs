using System.Threading.Tasks;

namespace Hookr.Web.Backend.Operations.Base
{
    public interface ICommandHandler<in TCommand> : IHandler
    {
        public Task ExecuteCommandAsync(TCommand command);
    }
}