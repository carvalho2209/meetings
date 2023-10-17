﻿using MediatR;
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

    protected ActionResult HandleFailure(Result result) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            IValidationResult validationResult =>
                BadRequest(
                    CreateProblemDetails(
                        "Validation Error", StatusCodes.Status400BadRequest,
                        result.Error,
                        validationResult.Errors)),
            _ =>
                BadRequest(
                    CreateProblemDetails(
                        "Bad Request",
                        StatusCodes.Status400BadRequest,
                        result.Error))
        };

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        Error[]? errors = null) =>
        new()
        {
            Title = title,
            Type = error.Code,
            Detail = error.Message,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };
}