using System;

namespace HookrTelegramBot.Operations
{
    public interface ICommandsContainer
    {
        Type TryGetByCommandName(string commandName);
    }
}