using MediatR;
using Meeting.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Meeting.Api.Controllers;

[ApiController]
[Route("[controller]/[action]/")]
public class ApiController : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()
                                                  ?? throw new ArgumentException(
                                                      "Unable to resolve IMediator instance. Application is misconfigured.");

    protected IActionResult HandleFailure(Result result) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            _ =>
                BadRequest(
                    CreateProblemDetails(
                        "Bad Request",
                        "Bad Request",
                        "One or more errors occurred",
                        StatusCodes.Status400BadRequest,
                        result.Errors))
        };

    private static ProblemDetails CreateProblemDetails(
        string title,
        string type,
        string detail,
        int status,
        Error[]? errors = null) =>
        new()
        {
            Title = title,
            Type = type,
            Detail = detail,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };
}