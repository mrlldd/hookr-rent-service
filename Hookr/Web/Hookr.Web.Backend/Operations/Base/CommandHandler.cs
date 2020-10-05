using System.Threading.Tasks;

namespace Hookr.Web.Backend.Operations.Base
{
    public abstract class CommandHandler<TCommand> : Handler, ICommandHandler<TCommand>
    {
        public abstract Task ExecuteCommandAsync(TCommand command);
    }
}