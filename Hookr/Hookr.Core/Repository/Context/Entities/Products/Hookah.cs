using System.Collections.Generic;
using Hookr.Core.Repository.Context.Entities.Products.Photo;

namespace Hookr.Core.Repository.Context.Entities.Products
{
    public class Hookah : Product
    {
        public ICollection<HookahPhoto>? Photos { get; set; }
    }
}