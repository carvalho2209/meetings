using Meeting.Application.Abstractions.Messaging;

namespace Meeting.Application.Members.Commands.CreateMember;

public sealed record CreateMemberCommand(
    string Email,
    string FirstName,
    string LastName) : ICommand<Guid>;
