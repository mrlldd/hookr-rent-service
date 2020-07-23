using System.Collections;
using System.Collections.Generic;
using HookrTelegramBot.Repository.Context.Entities.Base;

namespace HookrTelegramBot.Repository.Context.Entities
{
    public class Order : Entity
    {
        public ICollection<OrderedHookah> OrderedHookahs { get; set; }
        public ICollection<OrderedTobacco> OrderedTobaccos { get; set; }
    }
}