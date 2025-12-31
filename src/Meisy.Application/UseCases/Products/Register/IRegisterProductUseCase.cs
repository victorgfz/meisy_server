using Meisy.Communication.Requests;
using Meisy.Communication.Responses;

namespace Meisy.Application.UseCases.Products.Register
{
    public interface IRegisterProductUseCase
    {
        Task<ResponseProductJson> Execute(RequestRegisterProductJson request);
    }
}
