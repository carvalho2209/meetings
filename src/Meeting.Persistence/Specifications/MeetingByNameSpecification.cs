namespace Meeting.Persistence.Specifications;

internal class MeetingByNameSpecification : Specification<Domain.Entities.Meeting>
{
    public MeetingByNameSpecification(string name)
        : base(meeting => string.IsNullOrEmpty(name) ||
                          meeting.Name.Contains(name))
    {
        AddInclude(meeting => meeting.Creator);
        AddInclude(meeting => meeting.Attendees);

        AddOrderBy(meeting => meeting.Name);
    }
}
