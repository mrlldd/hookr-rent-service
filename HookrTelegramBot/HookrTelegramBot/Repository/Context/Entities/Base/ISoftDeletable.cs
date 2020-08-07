using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HookrTelegramBot.Repository.Context.Entities.Base
{
    public interface ISoftDeletable
    {
        DateTime? DeletedAt { get; set; }
        bool IsDeleted { get; set; }
        TelegramUser DeletedBy { get; set; }
        [ForeignKey(nameof(DeletedBy))]
        int? DeletedById { get; set; }
    }
}