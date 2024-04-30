using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Filters;

public interface IChainService
{
    ActionResult Execute(List<IError> errors);
}