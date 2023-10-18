namespace Meeting.Persistence.Specifications;

internal class MeetingByIdSplitSpecification : Specification<Domain.Entities.Meeting>
{
    public MeetingByIdSplitSpecification(Guid meetingId)
        : base(meeting => meeting.Id == meetingId)
    {
        AddInclude(meeting => meeting.Creator);
        AddInclude(meeting => meeting.Attendees);
        AddInclude(meeting => meeting.Invitations);

        IsSplitQuery = true;
    }
}