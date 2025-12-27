using System.Net;

namespace Meisy.Exception.ExceptionBase
{
    public class InvalidLoginException : MeisyException
    {
        public InvalidLoginException() : base(ResourceErrorMessages.INVALID_CREDENTIALS)
        {

        }
        public override int StatusCode => (int)HttpStatusCode.Unauthorized;
        public override List<string> GetErrors()
        {
            return [Message];
        }
    }
}
