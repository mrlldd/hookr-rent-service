using System;
using System.Threading;
using System.Threading.Tasks;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Web.Backend.Filters;
using Hookr.Web.Backend.Filters.Auth;
using Hookr.Web.Backend.Filters.Response.Auth;
using Hookr.Web.Backend.Models.Auth;
using Hookr.Web.Backend.Operations;
using Hookr.Web.Backend.Operations.Commands.Auth;
using Hookr.Web.Backend.Operations.Queries.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hookr.Web.Backend.Controllers
{
    [Route("api/auth")]
    [AuthResponseFilter]
    [ForAll]
    public class AuthController : Controller
    {
        private readonly Dispatcher dispatcher;

        public AuthController(Dispatcher dispatcher)
            => this.dispatcher = dispatcher;

        [HttpPost]
        [AllowAnonymous]
        public Task<JwtInfo> Confirm([FromBody] CreateSessionQuery query, CancellationToken token)
            => dispatcher
                .DispatchQueryAsync<CreateSessionQuery, JwtInfo>(query, token);

        [HttpPost("refresh")]
        public Task GenerateRefreshToken(CancellationToken token)
            => dispatcher
                .DispatchCommandAsync(new GenerateRefreshTokenCommand(), token);

        [HttpGet("refresh")]
        public Task<Guid> GetRefreshToken(CancellationToken token)
            => dispatcher
                .DispatchQueryAsync<GetRefreshTokenQuery, Guid>(new GetRefreshTokenQuery(), token);

        [HttpPost("refresh/{id:guid}")]
        [AllowAnonymous]
        public Task<JwtInfo> RefreshSession([FromRoute] Guid id, CancellationToken token)
            => dispatcher.DispatchQueryAsync<RefreshSessionQuery, JwtInfo>(new RefreshSessionQuery
            {
                RefreshToken = id
            }, token);

        [HttpDelete("refresh/{id:guid}")]
        public Task RevokeToken([FromRoute] Guid id, CancellationToken token)
            => dispatcher
                .DispatchCommandAsync(new RevokeTokenCommand
                {
                    Token = id
                }, token);
    }
}