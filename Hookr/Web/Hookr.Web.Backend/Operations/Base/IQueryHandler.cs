using System.Threading.Tasks;

namespace Hookr.Web.Backend.Operations.Base
{
    public interface IQueryHandler<in TQuery, TResult> : IHandler
    {
        Task<TResult> ExecuteQueryAsync(TQuery query);
    }
}