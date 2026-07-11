using Meisy.Communication.Requests.Auth;
using Meisy.Communication.Responses.Auth;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.User;
using Meisy.Domain.Security.Cryptography;
using Meisy.Domain.Security.Token;
using Meisy.Exception.ExceptionBase;

namespace Meisy.Application.UseCases.Auth.Login
{
    public class LoginUseCase : ILoginUseCase
    {

        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IPasswordEncrypter _bcrypt;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IUnitOfWork _unitOfWork;
        public LoginUseCase(
            IUserReadRepository userReadRepository,
            IUserWriteRepository userWriteRepository,
            IPasswordEncrypter bcrypt,
            ITokenGenerator tokenGenerator,
            IUnitOfWork unitOfWork
            )
        {

            _tokenGenerator = tokenGenerator;
         _bcrypt = bcrypt;
         _userReadRepository = userReadRepository;
         _userWriteRepository = userWriteRepository;
         _unitOfWork = unitOfWork;
        }

        public async Task<ResponseLoginJson> Execute(RequestLoginJson request)
        {

            var user = await _userReadRepository.GetByEmail(request.Email) ?? throw new InvalidLoginException(); ;
           
            var passwordMatch = _bcrypt.Verify(request.Password, user.Password);

            if (!passwordMatch)
            {
                throw new InvalidLoginException();
            }

            var refreshToken = _tokenGenerator.GenerateRefreshToken();
            
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(RefreshTokenPolicy.InactivityExpirationDays);

            _userWriteRepository.Update(user);
            await _unitOfWork.Commit();

            return new ResponseLoginJson
            {
                Name = user.Name,
                CompanyCode = user.Company.Code,
                Token = _tokenGenerator.GenerateToken(user),
                RefreshToken = refreshToken
            };
        }
    }
}
