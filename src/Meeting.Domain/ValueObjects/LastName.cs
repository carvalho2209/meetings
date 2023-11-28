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
        Result.Ensure(
                value,
                (e => !string.IsNullOrWhiteSpace(e),
                    DomainErrors.LastName.Empty),
                (e => e.Length <= MaxLength,
                    DomainErrors.LastName.TooLong))
            .Map(e => new LastName(e));


    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}