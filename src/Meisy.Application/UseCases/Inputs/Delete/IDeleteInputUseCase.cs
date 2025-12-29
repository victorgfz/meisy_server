namespace Meisy.Application.UseCases.Inputs.Delete
{
    public interface IDeleteInputUseCase
    {
        Task Execute(int id);
    }
}
