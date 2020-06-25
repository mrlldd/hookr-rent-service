using System.Threading.Tasks;

namespace HookrTelegramBot.Operations
{
    public interface ICommand
    {
        string Name { get; set; }
        Task ExecuteAsync();
    }
}