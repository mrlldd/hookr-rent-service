using System.Collections.Generic;
using Hookr.Telegram.Repository.Context.Entities.Products.Photo;

namespace Hookr.Telegram.Repository.Context.Entities.Products
{
    public class Hookah : Product
    {
        public ICollection<HookahPhoto>? Photos { get; set; }
    }
}