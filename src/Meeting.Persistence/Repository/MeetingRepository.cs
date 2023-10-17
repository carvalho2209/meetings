using Meeting.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Meeting.Persistence.Repository;

public sealed class MeetingRepository : IMeetingRepository
{
    private readonly ApplicationDbContext _context;

    public MeetingRepository(ApplicationDbContext context) => _context = context;

    public async Task<Domain.Entities.Meeting?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Domain.Entities.Meeting? meeting = await
            _context.Set<Domain.Entities.Meeting>()
                .AsSingleQuery()
                .Include(x => x.Creator)
                .Include(x => x.Attendees)
                .Include(x => x.Invitations)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return meeting;
    }

    public async Task<Domain.Entities.Meeting?> GetByIdWithCreatorAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Domain.Entities.Meeting>()
            .Include(c => c.Creator)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<Domain.Entities.Meeting?> GetByIdWithInvitationsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Domain.Entities.Meeting>()
            .Include(c => c.Invitations)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public void Add(Domain.Entities.Meeting meeting)
    {
        _context.Set<Domain.Entities.Meeting>().Add(meeting);
    }

    public void Remove(Domain.Entities.Meeting meeting)
    {
        _context.Set<Domain.Entities.Meeting>().Remove(meeting);
    }
}
