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
public class MembersController : ApiController
{
    //[HasPermission(Permission.ReadMember)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetMemberById(Guid id, CancellationToken cancellationToken)
    {
        return await Result
            .Create(new GetMemberByIdQuery(id))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match(
                Ok,
                HandleFailure);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMembers(CancellationToken cancellationToken)
    {
        return await Result
            .Create(
                new GetMember())
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match(
                Ok,
                HandleFailure);
    }

    [HasPermission(Permission.ReadMember)]
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

    [HasPermission(Permission.UpdateMember)]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        return await Result
            .Create(
                new LoginCommand(
                    request.Email))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match(
                Ok,
                HandleFailure);
    }
}