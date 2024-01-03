using Meeting.Domain.Errors;
using Meeting.Domain.Shared;

namespace Meeting.Domain.ValueObjects;

public sealed record LastName
{
    public const int MaxLength = 50;

    private LastName(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }

    public static Result<LastName> Create(string value) =>
        Result.Ensure(
                value,
                (e => !string.IsNullOrWhiteSpace(e),
                    DomainErrors.LastName.Empty),
                (e => e.Length <= MaxLength,
                    DomainErrors.LastName.TooLong))
            .Map(e => new LastName(e));

}