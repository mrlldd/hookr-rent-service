using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Hookr.Web.Backend.Config.Telegram;
using Hookr.Web.Backend.Operations.Base;
using Hookr.Web.Backend.Utilities.Extensions;

namespace Hookr.Web.Backend.Operations.Queries.Auth
{
    public class ConfirmAuthQueryHandler : QueryHandler<ConfirmAuthQuery, bool>
    {
        private readonly ITelegramConfig telegramConfig;

        public ConfirmAuthQueryHandler(ITelegramConfig telegramConfig)
        {
            this.telegramConfig = telegramConfig;
        }

        public override Task<bool> ExecuteQueryAsync(ConfirmAuthQuery query)
            => DelegateFactory(query)
                .Map(Task.Run);

        private Func<bool> DelegateFactory(ConfirmAuthQuery query)
            =>
                () =>
                {
                    using var sha256 = SHA256.Create();
                    var secretKey = sha256
                        .ComputeHash(telegramConfig.Token.Utf8Bytes()); //todo pass bot token here from config
                    using var hmacsha256 = new HMACSHA256(secretKey);
                    var dataCheckStringHash = hmacsha256
                        .ComputeHash(query.Key.Utf8Bytes());
                    return new StringBuilder(dataCheckStringHash.Length *
                                             2) // as actually unit length will increase by two
                        .SideEffect(builder => dataCheckStringHash
                            .ForEach(b => builder.AppendFormat("{0:x2}", b))
                        )
                        .ToString()
                        .Equals(query.Hash);
                };
    }
}