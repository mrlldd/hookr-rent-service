using HookrTelegramBot.Repository.Context;

namespace HookrTelegramBot.Repository
{
    public interface IHookrRepository
    {
        HookrContext Context { get; }
    }
}