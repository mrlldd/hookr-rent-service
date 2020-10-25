using System.Threading;

namespace Hookr.Web.Backend.Operations.Base
{
    public abstract class Handler
    {
        protected CancellationToken Token { get; private set; }

        public void Populate(CancellationToken token)
        {
            Token = token;
        }
    }
}