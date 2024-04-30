using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Filters;

public interface IErrorFilter
{
    bool CanPerform(List<IError> errors);
    ActionResult GetResponse();
}