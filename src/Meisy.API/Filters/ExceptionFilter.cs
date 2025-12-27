using Meisy.Communication.Responses;
using Meisy.Exception;
using Meisy.Exception.ExceptionBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Meisy.API.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is MeisyException)
            {
                HandleProjectException(context);
            }
            else
            {
                ThrowUnknownError(context);
            }

        }

        private void HandleProjectException(ExceptionContext context)
        {
            var meisyException = (MeisyException)context.Exception;
            var errorResponse = new ResponseErrorJson(meisyException.GetErrors());

            context.HttpContext.Response.StatusCode = meisyException.StatusCode;
            context.Result = new ObjectResult(errorResponse);

        }
        private void ThrowUnknownError(ExceptionContext context)
        {

            var errorResponse = new ResponseErrorJson(ResourceErrorMessages.UNKNOWN_ERROR);

            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(errorResponse);
        }
    }
}
