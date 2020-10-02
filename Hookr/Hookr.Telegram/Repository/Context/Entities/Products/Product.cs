using System.Diagnostics.CodeAnalysis;
using Hookr.Telegram.Repository.Context.Entities.Base;

namespace Hookr.Telegram.Repository.Context.Entities.Products
{
    public class Product : Entity
    {
        [NotNull]
        public string? Name { get; set; }
        public int Price { get; set; }
    }
}