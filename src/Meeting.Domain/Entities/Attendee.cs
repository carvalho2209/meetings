namespace Meeting.Domain.Entities;

public class Attendee
{
    internal Attendee(Invitation invitation)
        : this()
    {
        MeetingId = invitation.MeetingId;
        MemberId = invitation.MemberId;
        CreatedOnUtc = DateTime.UtcNow;
    }

    private Attendee()
    {

    }

    public Guid MeetingId { get; private set; }
    public Guid MemberId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
}