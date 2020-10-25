using System.Threading;

namespace Hookr.Web.Backend.Operations.Base
{
    public interface IHandler
    {
        void Populate(CancellationToken token);
    }
}