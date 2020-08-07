using System;

namespace HookrTelegramBot.Repository.Context.Entities.Base
{
    public interface ISoftDeletable
    {
        DateTime DeletedAt { get; set; }
        bool IsDeleted { get; set; }
        TelegramUser DeletedBy { get; set; }
        int DeletedById { get; set; }
    }
}