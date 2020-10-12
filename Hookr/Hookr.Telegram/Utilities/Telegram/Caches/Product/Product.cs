namespace Hookr.Telegram.Utilities.Telegram.Caches.Product
{
    public struct Product
    {
        private int? Source { get; set; }
        
        public bool HasValue => Source.HasValue;

        // ReSharper disable once PossibleInvalidOperationException
        public int Value => Source.Value;
        
        public static implicit operator int?(Product source)
            => source.Source;

        public static implicit operator Product(int? source)
            => new Product
            {
                Source = source
            };

        public static implicit operator Product(int source)
            => new Product
            {
                Source = source
            };

        public override string ToString()
            => Source.ToString();
    }
}