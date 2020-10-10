using Hookr.Core.Repository.Context.Entities.Base;

namespace Hookr.Core.Repository.Context.Entities.Products.Ordered
{
    public class Ordered<TProduct> : Entity where TProduct : Product
    {
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int Count { get; set; }
        
        public int ProductId { get; set; }
        public TProduct? Product { get; set; }
    }
}