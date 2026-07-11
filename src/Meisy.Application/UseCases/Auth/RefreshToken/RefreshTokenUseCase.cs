using Meisy.Communication.Requests.Auth;
using Meisy.Communication.Responses.Auth;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.User;
using Meisy.Domain.Security.Token;
using Meisy.Exception.ExceptionBase;

namespace Meisy.Application.UseCases.Auth.RefreshToken
{
    public class RefreshTokenUseCase : IRefreshTokenUseCase
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenUseCase(
            IUserReadRepository userReadRepository,
            IUserWriteRepository userWriteRepository,
            ITokenGenerator tokenGenerator,
            IUnitOfWork unitOfWork)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
            _tokenGenerator = tokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseLoginJson> Execute(RequestRefreshTokenJson request)
        {
            var user = await _userReadRepository.GetByRefreshToken(request.RefreshToken);

            if (user == null)
            {
                throw new InvalidLoginException();
            }

            if (user.RefreshTokenExpiryTime is null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new InvalidLoginException();
            }

            var newRefreshToken = _tokenGenerator.GenerateRefreshToken();
            
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(RefreshTokenPolicy.InactivityExpirationDays);

            _userWriteRepository.Update(user);
            await _unitOfWork.Commit();

            return new ResponseLoginJson
            {
                Name = user.Name,
                CompanyCode = user.Company.Code,
                Token = _tokenGenerator.GenerateToken(user),
                RefreshToken = newRefreshToken
            };
        }
    }
}
