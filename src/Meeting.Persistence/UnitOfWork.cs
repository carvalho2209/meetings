using Meeting.Domain.Repositories;

namespace Meeting.Persistence;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context; 

    public UnitOfWork(ApplicationDbContext context) => _context = context;

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
