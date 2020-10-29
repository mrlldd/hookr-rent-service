using System.Threading;
using Microsoft.Extensions.Logging;

namespace Hookr.Web.Backend.Operations.Base
{
    public abstract class Handler
    {
        protected CancellationToken Token { get; private set; }
        protected ILogger<IHandler> Logger { get; private set; }

        public void Populate(ILogger<IHandler> logger, CancellationToken token)
        {
            Logger = logger;
            Token = token;
        }
    }
}