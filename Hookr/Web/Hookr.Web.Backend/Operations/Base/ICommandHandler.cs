using System.Threading.Tasks;

namespace Hookr.Web.Backend.Operations.Base
{
    public interface ICommandHandler<in TCommand>
    {
        public Task ExecuteCommandAsync(TCommand command);
    }
}