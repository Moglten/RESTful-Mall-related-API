using Mall_Related_API.Models;
using Mall_Related_API.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Mall_Related_API.Handler
{
    public class BasicAuthentictionHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly UnitOfWork _unitOfWork = new();
        private readonly IRepository<LoginAuth> _repository ;


        public BasicAuthentictionHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
            ):base(options, logger, encoder, clock)
        {
            _repository = _unitOfWork.GetRepositoryInstance<LoginAuth>();
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!(Request.Headers.ContainsKey("Authorization")))
                return Task.FromResult(AuthenticateResult.Fail("Authorization Header wasn't found"));
            
            try
            {
                var AuthHeaderValue = Request.Headers["Authorization"];
                var bytes = Convert.FromBase64String(AuthHeaderValue);
                var credentials = Encoding.UTF8.GetString(bytes).Split(":");
                var User = _repository
                                .GetAllEntitiesIQueryable()
                                .Where(user => user.email == credentials[0] && user.PW == credentials[1])
                                .FirstOrDefault();
                if (User == null)
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid username of Password"));
                }
                else
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, User.email) };
                    var Identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(Identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }
            }
            catch
            {
                return Task.FromResult(AuthenticateResult.Fail("Error Has Occured"));
            }
            

        }
    }
}
