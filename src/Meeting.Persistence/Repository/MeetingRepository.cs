using Meeting.Domain.Repositories;

namespace Meeting.Persistence.Repository;

internal sealed class MeetingRepository : IMeetingRepository
{
    public async Task<Domain.Entities.Meeting?> GetByIdWithCreatorAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;

        return null;
    }

    public void Add(Domain.Entities.Meeting meeting)
    {
        
    }
}
