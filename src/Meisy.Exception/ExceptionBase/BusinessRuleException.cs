
using System.Net;

namespace Meisy.Exception.ExceptionBase
{
    public class BusinessRuleException : MeisyException
    {
        public BusinessRuleException(string message) : base(message)
        {

        }

        public override int StatusCode => (int)HttpStatusCode.Conflict;

        public override List<string> GetErrors()
        {
            return [Message];
        }
    }
}
