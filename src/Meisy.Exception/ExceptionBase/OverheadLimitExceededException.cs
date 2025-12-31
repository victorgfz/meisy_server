
using System.Net;

namespace Meisy.Exception.ExceptionBase
{
    public class OverheadLimitExceededException : MeisyException
    {
        public OverheadLimitExceededException(string message) : base(message)
        {

        }

        public override int StatusCode => (int)HttpStatusCode.Conflict;

        public override List<string> GetErrors()
        {
            return [Message];
        }
    }
}
