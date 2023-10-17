using Meeting.Application.Abstractions;
using Meeting.Application.Abstractions.Messaging;
using Meeting.Domain.DomainEvents;
using Meeting.Domain.Entities;
using Meeting.Domain.Repositories;

namespace Meeting.Application.Members.Events;

internal sealed class MemberRegisteredDomainEventHandler :
    IDomainEventHandler<MemberRegisteredDomainEvent>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IEmailService _emailService;

    public MemberRegisteredDomainEventHandler(IMemberRepository memberRepository, IEmailService emailService)
    {
        _memberRepository = memberRepository;
        _emailService = emailService;
    }

    public async Task Handle(MemberRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        Member? member = await _memberRepository.GetByIdAsync(notification.MemberId, cancellationToken);

        if (member == null)
        {
            return;
        }

        await _emailService.SendWelcomeEmailAsync(member, cancellationToken);
    }
}
