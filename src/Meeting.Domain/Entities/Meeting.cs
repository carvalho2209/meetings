using Meeting.Domain.DomainEvents;
using Meeting.Domain.Enums;
using Meeting.Domain.Errors;
using Meeting.Domain.Exceptions;
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

    private Meeting()
    {
    }

    public Member Creator { get; private set; }
    public MeetingType Type { get; private set; }
    public string Name { get; private set; }
    public DateTime ScheduleAtUtc { get; private set; }
    public string? Location { get; private set; }
    public int? MaximumNumberOfAttendees { get; private set; }
    public DateTime? InvitationsExpireAtUtc { get; private set; }
    public int NumberOfAttendees { get; private set; }
    public bool? Cancelled { get; set; }

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

        RaiseDomainEvent(new InvitationAcceptedDomainEvent(Guid.NewGuid(), invitation.Id, Id));

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

    public static Meeting Create(
        Guid id,
        Member creator,
        MeetingType type,
        DateTime scheduleAtUct,
        string name,
        string? location,
        int? maximumNumberOfAttendees,
        int? invitationsValidBeforeInHours)
    {
        var meeting = new Meeting(
            id,
            creator,
            type,
            scheduleAtUct,
            name,
            location);

        meeting.CalculateMeetingTypeDetails(maximumNumberOfAttendees, invitationsValidBeforeInHours);

        return meeting;
    }

    private void CalculateMeetingTypeDetails(int? maximumNumberOfAttendees, int? invitationsValidBeforeInHours)
    {
        switch (Type)
        {
            case MeetingType.WithFixedNumberOfAttendees:
                if (maximumNumberOfAttendees is null)
                {
                    throw new MeetingMaximumNumberOfAttendeesIsNullDomainException
                        ($"{nameof(maximumNumberOfAttendees)} can't be null.");
                }

                MaximumNumberOfAttendees = maximumNumberOfAttendees;
                break;

            case MeetingType.WithExpirationForInvitations:
                if (invitationsValidBeforeInHours is null)
                {
                    throw new MeetingInvitationsValidBeforeInHoursIsNullDomainException
                        ($"{nameof(invitationsValidBeforeInHours)} can't be null.");
                }

                InvitationsExpireAtUtc = ScheduleAtUtc.AddHours(-invitationsValidBeforeInHours.Value);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(MeetingType));
        }
    }

    public Result Cancel(DateTime utcNow)
    {
        if (utcNow >= ScheduleAtUtc)
        {
            return Result.Failure(DomainErrors.Meeting.AlreadyPassed);
        }

        if (Type == MeetingType.WithExpirationForInvitations &&
            utcNow >= InvitationsExpireAtUtc)
        {
            foreach (Invitation invitation in _invitations
                         .Where(i => i.Status == InvitationStatus.Pending))
            {
                invitation.Expire();
            }
        }

        Cancelled = true;

        RaiseDomainEvent(new OrderCancelledDomainEvent(Guid.NewGuid(), Id));

        return Result.Success();
    }
}