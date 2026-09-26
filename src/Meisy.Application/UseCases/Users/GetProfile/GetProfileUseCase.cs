using Meisy.Communication.Responses.Users;
using Meisy.Domain.Repositories.User;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Exception;
using Meisy.Exception.ExceptionBase;

namespace Meisy.Application.UseCases.Users.GetProfile
{
    public class GetProfileUseCase : IGetProfileUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUserReadRepository _userReadRepository;

        public GetProfileUseCase(ILoggedUser loggedUser, IUserReadRepository userReadRepository)
        {
            _loggedUser = loggedUser;
            _userReadRepository = userReadRepository;
        }

        public async Task<ResponseProfileJson> Execute()
        {
            var companyId = _loggedUser.GetCompanyId();
            var userId = _loggedUser.GetUserId();

            var user = await _userReadRepository.GetByIdWithCompany(companyId, userId) ?? throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);

            var ranking = await _userReadRepository.GetOrdersRanking(companyId);
            var loggedUserSummary = ranking.FirstOrDefault(r => r.UserId == userId);

            return new ResponseProfileJson
            {
                Name = user.Name,
                Email = user.Email,
                CompanyCode = user.Company.Code,
                QuantityOfOrders = loggedUserSummary?.QuantityOfOrders ?? 0,
                TotalRevenue = loggedUserSummary?.TotalRevenue ?? 0,
                CompanyUsers = ranking
                    .Select(r => new ResponseProfileCompanyUserJson
                    {
                        Id = r.UserId,
                        Name = r.Name,
                        QuantityOfOrders = r.QuantityOfOrders,
                        IsLoggedUser = r.UserId == userId
                    })
                    .ToList()
            };
        }
    }
}
