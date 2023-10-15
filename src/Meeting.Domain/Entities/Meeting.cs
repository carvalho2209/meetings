using Meeting.Domain.DomainEvents;
using Meeting.Domain.Enums;
using Meeting.Domain.Errors;
using Meeting.Domain.Primitives;
using Meeting.Domain.Shared;

namespace Meeting.Domain.Entities;

public class Meeting : AggregateRoot
{
    private readonly List<Invitation> _invitations = new();
    private readonly List<Attendee> _attendees = new();

    private Meeting(
        Guid id,
        Member creator,
        MeetingType type,
        DateTime scheduleAtUtc,
        string name,
        string? location)
        : base(id)
    {
        Creator = creator;
        Type = type;
        ScheduleAtUtc = scheduleAtUtc;
        Name = name;
        Location = location;
    }

    private Meeting() { }

    public Member Creator { get; private set; }
    public MeetingType Type { get; private set; }
    public string? Name { get; private set; }
    public DateTime ScheduleAtUtc { get; private set; }
    public string? Location { get; private set; }
    public int? MaximumNumberOfAttendees { get; private set; }
    public DateTime? InvitationsExpireAtUtc { get; private set; }
    public int NumberOfAttendees { get; private set; }

    public IReadOnlyCollection<Attendee> Attendees => _attendees;
    public IReadOnlyCollection<Invitation> Invitations => _invitations;

    public Result<Attendee> AcceptInvitation(Invitation invitation)
    {
        bool reachedMaximumNumberOfAttendees =
            Type == MeetingType.WithFixedNumberOfAttendees &&
            NumberOfAttendees == MaximumNumberOfAttendees;

        bool reachedInvitationsExpiration = 
            Type == MeetingType.WithExpirationForInvitations &&
            InvitationsExpireAtUtc < DateTime.UtcNow;

        bool expired = reachedMaximumNumberOfAttendees ||
                       reachedInvitationsExpiration;

        if (expired)
        {
            invitation.Expire();

            Result.Failure<Attendee>(DomainErrors.Meeting.Expired);
        }

        Attendee attendee = invitation.Accept();

        RaiseDomainEvent(new InvitationAcceptedDomainEvent(invitation.Id, Id));

        _attendees.Add(attendee);
        NumberOfAttendees++;

        return attendee;
    }

    public Result<Invitation> SendInvitation(Member member)
    {
        if (Creator.Id == member.Id)
        {
            return Result.Failure<Invitation>(DomainErrors.Meeting.InvitingCreator);
        }

        if (ScheduleAtUtc < DateTime.UtcNow)
        {
            return Result.Failure<Invitation>(DomainErrors.Meeting.AlreadyPassed);
        }

        var invitation = new Invitation(Guid.NewGuid(), member, this);

        _invitations.Add(invitation);

        return invitation;
    }
}