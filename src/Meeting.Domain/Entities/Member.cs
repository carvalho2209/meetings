using Meeting.Domain.Errors;
using Meeting.Domain.Primitives;
using Meeting.Domain.ValueObjects;

namespace Meeting.Domain.Entities;

public class Member : AggregateRoot
{
    private Member(Guid id, Email email, FirstName firstName, LastName lastName) 
        : base(id)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }

    private Member() { }

    public Email Email { get; set; }
    public FirstName FirstName { get; set; }
    public LastName LastName { get; set; }
}
