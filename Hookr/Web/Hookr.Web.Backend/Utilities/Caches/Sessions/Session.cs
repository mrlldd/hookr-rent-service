using System;
using Hookr.Core.Repository.Context.Entities.Base;

namespace Hookr.Web.Backend.Utilities.Caches.Sessions
{
    public class Session : TelegramUser
    {
        public Guid Key { get; set; }
    }
}