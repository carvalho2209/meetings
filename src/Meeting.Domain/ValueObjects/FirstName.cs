using Meeting.Domain.Errors;
using Meeting.Domain.Shared;

namespace Meeting.Domain.ValueObjects;

public sealed class FirstName
{
    public FirstName(string value) => Value = value;
    public const int MaxLength = 50;

    public string Value { get; }

    public static FirstName Create(string firstName)
    {
        Ensure.NotNullOrWhiteSpace(firstName, DomainErrors.FirstName.Empty);
        Ensure.NotGreaterThan(firstName.Length, MaxLength, DomainErrors.FirstName.TooLong);

        return new FirstName(firstName);
    }
}