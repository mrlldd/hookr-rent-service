using System.Threading.Tasks;
using Hookr.Web.Backend.Operations;
using Hookr.Web.Backend.Operations.Queries.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Hookr.Web.Backend.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly Dispatcher dispatcher;

        public AuthController(Dispatcher dispatcher) 
            => this.dispatcher = dispatcher;

        [HttpPost]
        public Task<bool> Confirm([FromBody] ConfirmAuthQuery query)
            => dispatcher
                .DispatchQueryAsync<ConfirmAuthQuery, bool>(query);
    }
}