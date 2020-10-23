using System.Threading.Tasks;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Web.Backend.Filters;
using Hookr.Web.Backend.Filters.Response.Auth;
using Hookr.Web.Backend.Operations;
using Hookr.Web.Backend.Operations.Queries.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hookr.Web.Backend.Controllers
{
    [Route("api/auth")]
    [AuthResponseFilter]
    [MinimumUserLevel(TelegramUserStates.Default)]
    public class AuthController : Controller
    {
        private readonly Dispatcher dispatcher;
        public AuthController(Dispatcher dispatcher) 
            => this.dispatcher = dispatcher;

        [HttpPost]
        public Task<ConfirmAuthQueryHandler.JwtTokens> Confirm([FromBody] ConfirmAuthQuery query)
            => dispatcher
                .DispatchQueryAsync<ConfirmAuthQuery, ConfirmAuthQueryHandler.JwtTokens>(query);
    }
}