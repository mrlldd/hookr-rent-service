using System.Collections.Generic;
using Hookr.Core.Repository.Context.Entities.Products.Photo;

namespace Hookr.Core.Repository.Context.Entities.Products
{
    public class Tobacco : Product
    {
        public ICollection<TobaccoPhoto>? Photos { get; set; }
    }
}