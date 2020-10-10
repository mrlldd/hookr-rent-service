namespace Hookr.Core.Utilities.Providers
{
    internal class TelegramUserIdProvider : ITelegramUserIdProvider
    {
        public void Set(int value)
        {
            ProvidedValue = value;
        }

        public int ProvidedValue { get; private set; }
    }
}