using System.Collections.Generic;
using HookrTelegramBot.Repository.Context.Entities.Products.Photo;

namespace HookrTelegramBot.Repository.Context.Entities.Products
{
    public class Tobacco : Product
    {
        public ICollection<TobaccoPhoto> Photos { get; set; }
    }
}