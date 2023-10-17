using Meeting.Application.Abstractions.Messaging;

namespace Meeting.Application.Members.Queries.GetMemberById;

public sealed record GetMemberByIdQuery(Guid MemberId) : IQuery<MemberResponse>;