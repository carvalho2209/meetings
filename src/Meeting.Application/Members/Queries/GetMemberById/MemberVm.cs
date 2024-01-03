using Meeting.Domain.Errors;
using Meeting.Domain.ValueObjects;

namespace Meeting.Application.Members.Queries.GetMemberById;

public record MemberVm(Guid Id, Email Email, FirstName FirstName, LastName LastName);

