﻿using System;
using Hookr.Telegram.Repository.Context.Entities.Base;

namespace Hookr.Telegram.Repository.Context.Entities.Products.Photo
{
    public class ProductPhoto : Entity, ISoftDeletable
    {
        public string? TelegramFileId { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public TelegramUser? DeletedBy { get; set; }
        public int? DeletedById { get; set; }
    }
}