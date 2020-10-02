using System.Threading.Tasks;

namespace Hookr.Telegram.Operations.Base
{
    public interface ICommand
    {
        public Task ExecuteAsync();
    }
}