using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Hookr.Core.Repository.Context.Entities.Base
{
    public class Entity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        [NotNull] public TelegramUser? CreatedBy { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [NotNull]
        public int? CreatedById { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public TelegramUser? UpdatedBy { get; set; }
        [ForeignKey(nameof(UpdatedBy))] public int? UpdatedById { get; set; }
    }
}