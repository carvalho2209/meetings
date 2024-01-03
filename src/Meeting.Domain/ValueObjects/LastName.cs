using Meeting.Domain.Errors;
using Meeting.Domain.Shared;

namespace Meeting.Domain.ValueObjects;

public sealed class LastName
{
    public LastName(string value) => Value = value;
    
    public const int MaxLength = 50;

    public string Value { get; }

    public static LastName Create(string lastName)
    {
        Ensure.NotNullOrWhiteSpace(lastName, DomainErrors.LastName.Empty);
        Ensure.NotGreaterThan(lastName.Length, MaxLength, DomainErrors.LastName.TooLong);

        return new LastName(lastName);
    }
}