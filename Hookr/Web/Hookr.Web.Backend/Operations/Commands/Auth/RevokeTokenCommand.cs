using System;
using Hookr.Web.Backend.Operations.Base;

namespace Hookr.Web.Backend.Operations.Commands.Auth
{
    public class RevokeTokenCommand
    {
        public Guid Token { get; set; }
    }
}