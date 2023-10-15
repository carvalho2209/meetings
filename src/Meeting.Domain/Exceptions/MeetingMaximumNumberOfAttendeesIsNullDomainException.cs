namespace Meeting.Domain.Exceptions;

public sealed class MeetingMaximumNumberOfAttendeesIsNullDomainException : DomainException
{
    public MeetingMaximumNumberOfAttendeesIsNullDomainException(string message) : base(message)
    {
    }
}
