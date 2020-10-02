using System;

namespace Hookr.Telegram.Operations
{
    public interface ICommandsContainer
    {
        Type? TryGetByCommandName(string? commandName);
    }
}