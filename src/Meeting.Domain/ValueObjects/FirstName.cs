using Meeting.Domain.Errors;
using Meeting.Domain.Shared;

namespace Meeting.Domain.ValueObjects;

public sealed record FirstName
{
    public const int MaxLength = 50;

    private FirstName(string value)
    {
        Value = value;
    }

    public FirstName()
    {
        
    }
    
    public string Value { get; private set; }

    public static Result<FirstName> Create(string value) =>
        Result.Ensure(
                value,
                (e => !string.IsNullOrWhiteSpace(e), DomainErrors.FirstName.Empty),
                (e => e.Length <= MaxLength, DomainErrors.FirstName.TooLong))
            .Map(e => new FirstName(e));
}