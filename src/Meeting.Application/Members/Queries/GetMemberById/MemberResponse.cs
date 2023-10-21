using Meeting.Domain.Entities;

namespace Meeting.Application.Members.Queries.GetMemberById;

public record MemberResponse(Guid Id, string Email);