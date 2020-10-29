using System.Threading;
using Microsoft.Extensions.Logging;

namespace Hookr.Web.Backend.Operations.Base
{
    public interface IHandler
    {
        void Populate(ILogger<IHandler> logger, CancellationToken token);
    }
}