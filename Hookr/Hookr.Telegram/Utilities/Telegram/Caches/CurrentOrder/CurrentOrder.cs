namespace Hookr.Telegram.Utilities.Telegram.Caches.CurrentOrder
{
    public struct CurrentOrder
    {
        private int? Source { get; set; }
        
        public bool HasValue => Source.HasValue;

        // ReSharper disable once PossibleInvalidOperationException
        public int Value => Source.Value;
        
        public static implicit operator int?(CurrentOrder source)
            => source.Source;

        public static implicit operator CurrentOrder(int? source)
            => new CurrentOrder
            {
                Source = source
            };

        public static implicit operator CurrentOrder(int source)
            => new CurrentOrder
            {
                Source = source
            };

        public override string ToString()
            => Source.ToString();
    }
}