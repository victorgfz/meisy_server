using Meisy.Communication.Requests.Auth;
using Meisy.Communication.Responses.Auth;
using Meisy.Domain.Repositories.User;
using Meisy.Domain.Security.Cryptography;
using Meisy.Domain.Security.Token;
using Meisy.Exception.ExceptionBase;

namespace Meisy.Application.UseCases.Auth.Login
{
    public class LoginUseCase : ILoginUseCase
    {

        private readonly IUserReadRepository _userReadRepository;
        private readonly IPasswordEncrypter _bcrypt;
        private readonly ITokenGenerator _tokenGenerator;
        public LoginUseCase(
            IUserReadRepository userReadRepository,
            IPasswordEncrypter bcrypt,
            ITokenGenerator tokenGenerator
            )
        {

            _tokenGenerator = tokenGenerator;
         _bcrypt = bcrypt;
         _userReadRepository = userReadRepository;
        }

        public async Task<ResponseLoginJson> Execute(RequestLoginJson request)
        {

            var user = await _userReadRepository.GetByEmail(request.Email) ?? throw new InvalidLoginException(); ;
           
            var passwordMatch = _bcrypt.Verify(request.Password, user.Password);

            if (!passwordMatch)
            {
                throw new InvalidLoginException();
            }


            return new ResponseLoginJson
            {
                Name = user.Name,
                CompanyCode = user.Company.Code,
                Token = _tokenGenerator.GenerateToken(user)
            };
        }
    }
}
