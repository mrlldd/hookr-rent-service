using System.Threading.Tasks;

namespace Hookr.Web.Backend.Operations.Base
{
    public interface IQueryHandler<in TQuery, TResult>
    {
        Task<TResult> ExecuteQueryAsync(TQuery query);
    }
}