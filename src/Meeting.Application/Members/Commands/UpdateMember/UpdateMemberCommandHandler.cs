using Meeting.Application.Abstractions.Messaging;
using Meeting.Domain.Errors;
using Meeting.Domain.Repositories;
using Meeting.Domain.Shared;
using Meeting.Domain.ValueObjects;

namespace Meeting.Application.Members.Commands.UpdateMember;

public sealed class UpdateMemberCommandHandler : ICommandHandler<UpdateMemberCommand>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMemberCommandHandler(IMemberRepository memberRepository, IUnitOfWork unitOfWork)
    {
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken);

        if (member is null)
        {
            return Result.Failure(DomainErrors.Member.NotFound(request.MemberId));
        }

        Result<FirstName> firstName = FirstName.Create(request.FirstName);
        Result<LastName> lastName = LastName.Create(request.LastName);

        member.ChangeName(firstName.Value, lastName.Value);

        _memberRepository.Update(member);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
