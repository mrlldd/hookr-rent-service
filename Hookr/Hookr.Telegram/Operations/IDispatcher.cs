using System.Threading.Tasks;

namespace Hookr.Telegram.Operations
{
    public interface IDispatcher
    {
        Task DispatchAsync();
    }
}