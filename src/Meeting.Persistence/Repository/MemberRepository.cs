using Meeting.Domain.Entities;
using Meeting.Domain.Repositories;
using Meeting.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Meeting.Persistence.Repository;

public sealed class MemberRepository : IMemberRepository
{
    private readonly ApplicationDbContext _context;

    public MemberRepository(ApplicationDbContext context) => _context = context;

    public async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _context.Set<Member>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default) =>
        !await _context.Set<Member>().AnyAsync(x => x.Email == email, cancellationToken);

    public void Add(Member member) => _context.Set<Member>().Add(member);
}