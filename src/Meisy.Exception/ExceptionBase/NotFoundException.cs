using System.Net;

namespace Meisy.Exception.ExceptionBase
{
    public class NotFoundException : MeisyException
    {
        public NotFoundException(string message) : base(message)
        {

        }

        public override int StatusCode => (int)HttpStatusCode.NotFound;

        public override List<string> GetErrors()
        {
            return [Message];
        }
    }
}
