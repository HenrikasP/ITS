using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Claims.Controllers;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ProblemDetails))]
[SwaggerResponse(StatusCodes.Status400BadRequest, "Bad Request", typeof(ProblemDetails))]
[SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(ProblemDetails))]
[SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden", typeof(ProblemDetails))]
public class DocumentedControllerBase : ControllerBase
{
}