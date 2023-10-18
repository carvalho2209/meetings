﻿using Meeting.Api.Contracts.Members;
using Meeting.Application.Meetings.Commands.CreateMeeting;
using Meeting.Application.Meetings.Commands.Queries.GetMeetingById;
using Meeting.Application.Members.Commands.CreateMember;
using Meeting.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Meeting.Api.Controllers;

public sealed class MeetingsController : ApiController
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetMeetingById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetMeetingByIdQuery(id);

        var response = await Mediator.Send(query, cancellationToken);

        return response.IsSuccess
            ? Ok(response)
            : NotFound(response.Error);
    }

    [HttpPost]
    public async Task<ActionResult> RegisterMeeting(
        [FromBody] CreateMeetingCommand request,
        CancellationToken cancellationToken)
    {
        //var command = new CreateMeetingCommand();

        Result<Guid> result = await Mediator.Send(request, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(
            nameof(GetMeetingById),
            new { id = result.Value },
            result.Value);
    }
}