namespace Meeting.Domain.Exceptions;

public sealed class MeetingInvitationsValidBeforeInHoursIsNullDomainException : DomainException
{
    public MeetingInvitationsValidBeforeInHoursIsNullDomainException(string message) : base(message)
    {
    }
}
