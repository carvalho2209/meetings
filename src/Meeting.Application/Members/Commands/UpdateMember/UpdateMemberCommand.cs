using Meeting.Application.Abstractions.Messaging;

namespace Meeting.Application.Members.Commands.UpdateMember;

public sealed record UpdateMemberCommand(Guid MemberId, string FirstName, string LastName) : ICommand;

