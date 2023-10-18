using Meeting.Application.Meetings.Commands.Queries.GetMeetingById;
using Meeting.Application.Members.Queries.GetMemberById;
using Microsoft.AspNetCore.Mvc;

namespace Meeting.Api.Controllers;

public sealed class MeetingsController : ApiController
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetMeetingById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetMeetingByIdQuery(id);

        var response = await Mediator.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response) : NotFound(response.Error);
    }
}
