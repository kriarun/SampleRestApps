using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SampleRestApps.Controller;

public class ApiExceptionFilter : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not ApiException ex) return;

        context.Result = ex.StatusCode switch
        {
            HttpStatusCode.BadRequest => new BadRequestObjectResult(new ProblemDetails { Detail = ex.Message }),
            HttpStatusCode.NotFound   => new NotFoundObjectResult(new ProblemDetails { Detail = ex.Message }),
            _                         => new ObjectResult(new ProblemDetails { Detail = "Downstream API unavailable." }) { StatusCode = 502 }
        };
        context.ExceptionHandled = true;
    }
}