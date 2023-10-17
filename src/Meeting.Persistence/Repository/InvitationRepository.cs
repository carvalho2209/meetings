using Meeting.Domain.Entities;
using Meeting.Domain.Repositories;

namespace Meeting.Persistence.Repository;

public sealed class InvitationRepository : IInvitationRepository
{
    private readonly ApplicationDbContext _context;

    public InvitationRepository(ApplicationDbContext context) => _context = context;

    public void Add(Invitation invitation)
        => _context.Set<Invitation>().Add(invitation);
}
