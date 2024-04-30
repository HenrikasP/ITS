using Claims.Application.Errors;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Filters;

public class NotFoundErrorFilter : IErrorFilter
{
    public bool CanPerform(List<IError> errors)
    {
        return errors.Any(x => x.GetType() == typeof(NotFoundError));
    }

    public ActionResult GetResponse()
    {
        return new NotFoundResult();
    }
}