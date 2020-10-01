namespace HookrTelegramBot.Repository.Context.Entities
{
    public enum OrderStates
    {
        Unknown = 0,
        Constructing,
        Confirmed,
        Approved,
        Rejected,
        Processing,
        Finished
    }
}