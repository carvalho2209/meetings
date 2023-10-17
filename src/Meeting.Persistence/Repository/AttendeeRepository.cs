using Meeting.Domain.Entities;
using Meeting.Domain.Repositories;

namespace Meeting.Persistence.Repository;

public sealed class AttendeeRepository : IAttendeeRepository
{
    private readonly ApplicationDbContext _context;

    public AttendeeRepository(ApplicationDbContext context) => _context = context;

    public void Add(Attendee attendee) => _context.Set<Attendee>().Add(attendee);
}
