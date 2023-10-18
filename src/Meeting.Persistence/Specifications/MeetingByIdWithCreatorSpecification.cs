namespace Meeting.Persistence.Specifications;

internal class MeetingByIdWithCreatorSpecification : Specification<Domain.Entities.Meeting>
{
    public MeetingByIdWithCreatorSpecification(Guid meetingId)
        : base(meeting => meeting.Id == meetingId)
    {
        AddInclude(meeting => meeting.Creator);
    }
}