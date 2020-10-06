namespace Hookr.Web.Backend.Filters.Response
{
    public abstract class Response
    {
        public string TraceId { get; set; } 
    }
    
    public class Success : Response 
    {
    }

    public class Success<T> : Success
    {
        public T Data { get; set; }
    }

    public class Error : Response
    {
        public string Type { get; set; }
        public string Description { get; set; }
    }
}