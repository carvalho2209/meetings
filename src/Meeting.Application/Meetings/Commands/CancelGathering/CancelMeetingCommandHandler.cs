using Meeting.Application.Abstractions.Messaging;
using Meeting.Application.Abstractions;
using Meeting.Domain.Errors;
using Meeting.Domain.Repositories;
using Meeting.Domain.Shared;

namespace Meeting.Application.Meetings.Commands.CancelGathering;

internal sealed class CancelMeetingCommandHandler : ICommandHandler<CancelMeetingCommand>
{
    private readonly IMeetingRepository _meetRepository;
    private readonly ISystemTimeProvider _systemTimeProvider;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;

    public CancelMeetingCommandHandler(
        IMeetingRepository meetRepository,
        ISystemTimeProvider systemTimeProvider,
        IEmailService emailService,
        IUnitOfWork unitOfWork)
    {
        _meetRepository = meetRepository;
        _systemTimeProvider = systemTimeProvider;
        _emailService = emailService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CancelMeetingCommand request, CancellationToken cancellationToken)
    {
        var meeting = await _meetRepository.GetByIdAsync( 
            request.GatheringId,
            cancellationToken);

        if (meeting is null)
        {
            return Result.Failure(DomainErrors.Meeting.NotFound(request.GatheringId));
        }

        Result result = meeting.Cancel(_systemTimeProvider.UtcNow);

        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
