using Meeting.Domain.Repositories;
using Meeting.Persistence.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Meeting.Persistence.Repository;

public sealed class MeetingRepository : IMeetingRepository
{
    private readonly ApplicationDbContext _context;

    public MeetingRepository(ApplicationDbContext context) => _context = context;

    public async Task<List<Domain.Entities.Meeting>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default) =>
        await ApplySpecification(new MeetingByNameSpecification(name))
            .ToListAsync(cancellationToken);

    public async Task<Domain.Entities.Meeting?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default) =>
        await ApplySpecification(new MeetingByIdSplitSpecification(id))
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<List<Domain.Entities.Meeting>> GetByCreatorIdAsync(
        Guid creatorId,
        CancellationToken cancellationToken = default)
    {
        var members = await _context
            .Set<Domain.Entities.Meeting>()
            .Where(x => x.Creator.Id == creatorId)
            .ToListAsync(cancellationToken);

        return members;
    }

    public async Task<Domain.Entities.Meeting?> GetByIdWithCreatorAsync(
        Guid id,
        CancellationToken cancellationToken = default) =>
        await ApplySpecification(new MeetingByIdWithCreatorSpecification(id))
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Domain.Entities.Meeting?> GetByIdWithInvitationsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<Domain.Entities.Meeting>()
            .Include(c => c.Invitations)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private IQueryable<Domain.Entities.Meeting> ApplySpecification(
        Specification<Domain.Entities.Meeting> specification)
    {
        return SpecificationEvaluator.GetQuery(
            _context.Set<Domain.Entities.Meeting>(),
            specification);
    }

    public void Add(Domain.Entities.Meeting meeting)
        => _context.Set<Domain.Entities.Meeting>().Add(meeting);

    public void Remove(Domain.Entities.Meeting meeting)
        => _context.Set<Domain.Entities.Meeting>().Remove(meeting);
}