using Meeting.Domain.Enums;
using Meeting.Domain.Primitives;
using Meeting.Domain.Shared;

namespace Meeting.Domain.Entities;

public class Invitation : Entity
{
    internal Invitation(Guid id, Member member, Meeting meeting)
        : base(id)
    {
        Ensure.NotNull(member);
        Ensure.NotNull(meeting);
        Status = InvitationStatus.Pending;
        CreatedOnUtc = DateTime.UtcNow;
        Member = member;
        Meeting = meeting;
    }

    private Invitation() { }

    public Guid MeetingId { get; private set; }

    public Guid MemberId { get; private set; }
    
    public Member Member { get; private set; }
    
    public Meeting Meeting { get; private set; }

    public InvitationStatus Status { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? ModifiedOnUtc { get; private set; }

    internal void Expire()
    {
        Status = InvitationStatus.Expired;
        ModifiedOnUtc = DateTime.UtcNow;
    }

    internal Attendee Accept()
    {
        Status = InvitationStatus.Accepted;
        ModifiedOnUtc = DateTime.UtcNow;

        var attendee = new Attendee(this);

        return attendee;
    }
}
