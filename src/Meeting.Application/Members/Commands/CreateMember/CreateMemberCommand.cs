using MediatR;
using Meeting.Application.Abstractions.Messaging;
using Meeting.Domain.Shared;

namespace Meeting.Application.Members.Commands.CreateMember;

public sealed record CreateMemberCommand(
    string Email,
    string FirstName,
    string LastName) : ICommand<Guid>;
