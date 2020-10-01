using HookrTelegramBot.Repository.Context.Entities.Base;

namespace HookrTelegramBot.Repository.Context.Entities.Products
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public int Price { get; set; }
    }
}