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
        Result.Create(value)
            .Ensure(v => !string.IsNullOrWhiteSpace(v),
                DomainErrors.FirstName.Empty)
            .Ensure(v => v.Length <= MaxLength,
                DomainErrors.FirstName.TooLong)
            .Map(v => new FirstName(v));

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
