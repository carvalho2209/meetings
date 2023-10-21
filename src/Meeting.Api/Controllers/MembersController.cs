using Meeting.Api.Contracts.Members;
using Meeting.Application.Members.Commands.CreateMember;
using Meeting.Application.Members.Commands.UpdateMember;
using Meeting.Application.Members.Login;
using Meeting.Application.Members.Queries.GetMemberById;
using Meeting.Domain.Enums;
using Meeting.Domain.Shared;
using Meeting.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Meeting.Api.Controllers;

[Route("api/members")]
public class MembersController : ApiController
{
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetMemberByIdQueryHandler>> GetMemberById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetMemberByIdQuery(id);

        var response = await Mediator.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response) : NotFound(response.Error);
    }

    [HttpGet]
    public async Task<ActionResult<MemberVm[]>> GetAllMembers(CancellationToken cancellationToken)
    {
        var query = new GetMember();

        var response = await Mediator.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response) : NotFound(response.Error);
    }

    [HasPermission(Permission.ReadMember)]
    [HttpPost]
    public async Task<ActionResult> RegisterMember(
        [FromBody] RegisterMemberRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateMemberCommand(
            request.Email,
            request.FirstName,
            request.LastName);

        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(
            nameof(GetMemberById),
            new { id = result.Value },
            result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateMember(
        Guid id,
        [FromBody] UpdateMemberRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateMemberCommand(id, request.FirstName, request.LastName);

        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }

    [HasPermission(Permission.UpdateMember)]
    [HttpPost("login")]
    public async Task<ActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email);

        var tokenResult = await Mediator.Send(command, cancellationToken);

        if (tokenResult.IsFailure)
        {
            return HandleFailure(tokenResult);
        }

        return Ok(tokenResult.Value);
    }
}
