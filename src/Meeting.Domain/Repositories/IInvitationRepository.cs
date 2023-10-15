using Meeting.Domain.Entities;

namespace Meeting.Domain.Repositories;

public interface IInvitationRepository
{
    void Add(Invitation  invitation);
}
