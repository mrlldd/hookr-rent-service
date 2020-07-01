using HookrTelegramBot.Repository.Context;

namespace HookrTelegramBot.Repository
{
    public class HookrRepository : IHookrRepository
    {
        public HookrContext Context { get; }

        public HookrRepository(HookrContext context)
        {
            Context = context;
        }
    }
}