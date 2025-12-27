namespace Meisy.Exception.ExceptionBase
{
    public abstract class MeisyException : SystemException
    {
        protected MeisyException(string message) : base(message)
        {

        }

        public abstract int StatusCode { get; }
        public abstract List<string> GetErrors();
    }
}
