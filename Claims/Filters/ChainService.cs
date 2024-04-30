using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Filters;

public class ChainService : IChainService
{
    private readonly IErrorFilter[] _errorFilters;

    public ChainService(params IErrorFilter[] errorFilters)
    {
        _errorFilters = errorFilters;
    }

    public ActionResult Execute(List<IError> errors)
    {
        foreach (var filter in _errorFilters)
        {
            if (filter.CanPerform(errors))
            {
                return filter.GetResponse();
            }
        }

        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}