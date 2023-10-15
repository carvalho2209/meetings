namespace Meeting.Domain.Repositories;

public interface IMeetingRepository
{
    Task<Entities.Meeting?> GetByIdWithCreatorAsync(Guid id, CancellationToken cancellationToken = default);

    void Add(Entities.Meeting meeting);

}
