using System.Threading.Tasks;

namespace Hookr.Web.Backend.Operations.Base
{
    public abstract class QueryHandler<TQuery, TResult> : Handler, IQueryHandler<TQuery, TResult>
    {
        public abstract Task<TResult> ExecuteQueryAsync(TQuery query);
    }
}