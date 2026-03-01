using AutoMapper;
using Meisy.Communication.Requests.Clients;
using Meisy.Communication.Responses.Clients;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Input;
using Meisy.Domain.Services.LoggedUser;

namespace Meisy.Application.UseCases.Clients.Register
{
    public interface IRegisterClientUseCase
    {
        

        Task<ResponseClientJson> Execute(RequestClientJson request); 
    }
}
