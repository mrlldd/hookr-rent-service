using System.ComponentModel.DataAnnotations.Schema;
using HookrTelegramBot.Repository.Context.Entities.Base;

namespace HookrTelegramBot.Repository.Context.Entities
{
    public class Ordered<TProduct> : Entity where TProduct : Product
    {
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [ForeignKey(nameof(Product))] 
        public int ProductId { get; set; }
        public TProduct Product { get; set; }
        public int Count { get; set; }
    }
}