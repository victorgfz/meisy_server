namespace Meisy.Domain.Security.Token
{
    public interface ITokenProvider
    {
        string TokenFromRequest();
    }
}
