using HookrTelegramBot.Repository.Context.Entities.Base;

namespace HookrTelegramBot.Repository.Context.Entities
{
    public class Hookah : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }
}