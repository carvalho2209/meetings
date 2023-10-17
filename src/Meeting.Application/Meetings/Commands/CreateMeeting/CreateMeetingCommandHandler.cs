using Meeting.Application.Abstractions.Messaging;
using Meeting.Domain.Errors;
using Meeting.Domain.Repositories;
using Meeting.Domain.Shared;

namespace Meeting.Application.Meetings.Commands.CreateMeeting;

public sealed class CreateMeetingCommandHandler : ICommandHandler<CreateMeetingCommand, Guid>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IMeetingRepository _meetRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMeetingCommandHandler(IMemberRepository memberRepository, IMeetingRepository meetRepository, IUnitOfWork unitOfWork)
    {
        _memberRepository = memberRepository;
        _meetRepository = meetRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateMeetingCommand request, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken);

        if (member == null)
        {
            return Result.Failure<Guid>(DomainErrors.Meeting.AlreadyPassed);
        }
        
        var meeting = Domain.Entities.Meeting.Create(
            Guid.NewGuid(),
            member,
            request.Type,
            request.ScheduleAtUct,
            request.Name,
            request.Location,
            request.MaximumNumberOfAttendees,
            request.InvitationsValidBeforeInHours);

        _meetRepository.Add(meeting);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return meeting.Id;
    }
}
