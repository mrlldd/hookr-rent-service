using System.Collections;
using System.Collections.Generic;
using HookrTelegramBot.Repository.Context.Entities.Products.Photo;

namespace HookrTelegramBot.Repository.Context.Entities.Products
{
    public class Hookah : Product
    {
        public ICollection<HookahPhoto> Photos { get; set; }
    }
}