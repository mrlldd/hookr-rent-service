using System.Collections.Generic;
using Hookr.Telegram.Repository.Context.Entities.Products.Photo;

namespace Hookr.Telegram.Repository.Context.Entities.Products
{
    public class Tobacco : Product
    {
        public ICollection<TobaccoPhoto>? Photos { get; set; }
    }
}