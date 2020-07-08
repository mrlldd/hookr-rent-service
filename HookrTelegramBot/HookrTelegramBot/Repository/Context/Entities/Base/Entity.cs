using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HookrTelegramBot.Repository.Context.Entities.Base
{
    public class Entity
    {
        public DateTime CreatedAt { get; set; }
        public TelegramUser CreatedBy { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public int? CreatedById { get; set; }
        public DateTime UpdatedAt { get; set; }
        public TelegramUser UpdatedBy { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        public int? UpdatedById { get; set; }
    }
}