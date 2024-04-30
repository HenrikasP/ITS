using Claims.Application.Errors;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Filters;

public class BadRequestErrorFilter : IErrorFilter
{
    public bool CanPerform(List<IError> errors)
    {
        return errors.Any(x => x.GetType() == typeof(BadRequestError));
    }

    public ActionResult GetResponse()
    {
        return new BadRequestResult();
    }
}