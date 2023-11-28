using Meeting.Domain.Errors;
using Meeting.Domain.Primitives;
using Meeting.Domain.Shared;

namespace Meeting.Domain.ValueObjects;

public sealed class FirstName : ValueObject
{
    public FirstName(string value) => Value = value;
    public const int MaxLength = 50;

    public string Value { get; }

    public static Result<FirstName> Create(string value) =>
        Result.Ensure(
                value,
                (e => !string.IsNullOrWhiteSpace(e), DomainErrors.FirstName.Empty),
                (e => e.Length <= MaxLength, DomainErrors.FirstName.TooLong))
            .Map(e => new FirstName(e));

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}