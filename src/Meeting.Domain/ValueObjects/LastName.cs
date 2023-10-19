using Meeting.Domain.Errors;
using Meeting.Domain.Primitives;
using Meeting.Domain.Shared;

namespace Meeting.Domain.ValueObjects;

public sealed class LastName : ValueObject
{
    public LastName(string value) => Value = value;
    public const int MaxLength = 50;

    public string Value { get; }

    public static Result<LastName> Create(string value) =>
        Result.Create(value)
            .Ensure(v => !string.IsNullOrWhiteSpace(v),
                DomainErrors.LastName.Empty)
            .Ensure(v => v.Length <= MaxLength,
                DomainErrors.LastName.TooLong)
            .Map(v => new LastName(v));

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
