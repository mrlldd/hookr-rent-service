using System.Diagnostics.CodeAnalysis;
using Hookr.Core.Repository.Context.Entities.Base;

namespace Hookr.Core.Repository.Context.Entities.Products
{
    public class Product : Entity
    {
        [NotNull]
        public string? Name { get; set; }
        public int Price { get; set; }
    }
}