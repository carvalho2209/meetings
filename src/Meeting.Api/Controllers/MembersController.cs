using Meeting.Api.Contracts.Members;
using Meeting.Api.Extensions;
using Meeting.Application.Members.Commands.CreateMember;
using Meeting.Application.Members.Commands.UpdateMember;
using Meeting.Application.Members.Login;
using Meeting.Application.Members.Queries.GetMemberById;
using Meeting.Domain.Enums;
using Meeting.Domain.Shared;
using Meeting.Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Meeting.Api.Controllers;

[Route("api/members")]
public sealed class MembersController : ApiController
{
    [HttpGet]
    public async Task<IActionResult> GetMember(CancellationToken cancellationToken)
    {
        var query = new GetMember();

        var response = await Mediator.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Errors);
    }

    [HasPermission(Permission.ReadMember)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetMemberById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetMemberByIdQuery(id);

        Result<MemberResponse> response = await Mediator.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginMember(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email);

        Result<string> tokenResult = await Mediator.Send(
            command,
            cancellationToken);

        if (tokenResult.IsFailure)
        {
            return HandleFailure(tokenResult);
        }

        return Ok(tokenResult.Value);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterMember(
        [FromBody] RegisterMemberRequest request,
        CancellationToken cancellationToken)
    {
        return await Result
            .Create(
                new CreateMemberCommand(
                    request.Email,
                    request.FirstName,
                    request.LastName))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match(
                id => CreatedAtAction(nameof(GetMemberById), new { id }, id),
                HandleFailure);
    }

    [HasPermission(Permission.UpdateMember)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateMember(
        Guid id,
        [FromBody] UpdateMemberRequest request,
        CancellationToken cancellationToken)
    {
        return await Result
            .Create(
                new UpdateMemberCommand(
                    id,
                    request.FirstName,
                    request.LastName))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match(
                NoContent,
                HandleFailure);
    }
}