using System.Diagnostics.CodeAnalysis;

namespace Hookr.Web.Backend.Filters.Response
{
    public abstract class Response
    {
        [NotNull]public string? TraceId { get; set; } 
    }
    
    public class Success : Response 
    {
    }

    public class Success<T> : Success
    {
        [AllowNull]
        public T Data { get; set; }
    }

    public class Error : Response
    {
        [NotNull]public string? Type { get; set; }
        [NotNull]public string? Description { get; set; }
    }
}