using System;
using System.Collections.Generic;
using Hookr.Telegram.Repository.Context.Entities.Base;
using Hookr.Telegram.Repository.Context.Entities.Products.Ordered;

namespace Hookr.Telegram.Repository.Context.Entities
{
    public class Order : Entity, ISoftDeletable
    {
        public ICollection<OrderedHookah>? OrderedHookahs { get; set; }
        public ICollection<OrderedTobacco>? OrderedTobaccos { get; set; }
        public OrderStates State { get; set; }
        
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public TelegramUser? DeletedBy { get; set; }
        public int? DeletedById { get; set; }
    }
}