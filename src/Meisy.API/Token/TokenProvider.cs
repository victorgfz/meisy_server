using Meisy.Domain.Security.Token;

namespace Meisy.API.Token
{
    public class TokenProvider : ITokenProvider
    {

        private readonly IHttpContextAccessor _contextAccessor;
        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public string TokenFromRequest()
        {
            var authorization = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();
            return authorization["Bearer ".Length..].Trim();
        }
    }
}
