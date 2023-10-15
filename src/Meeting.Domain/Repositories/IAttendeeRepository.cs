using Meeting.Domain.Entities;

namespace Meeting.Domain.Repositories;

public interface IAttendeeRepository
{
    void Add(Attendee attendee);
}
