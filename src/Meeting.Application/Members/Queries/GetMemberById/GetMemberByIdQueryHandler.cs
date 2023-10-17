using Meeting.Application.Abstractions.Messaging;
using Meeting.Domain.Repositories;
using Meeting.Domain.Shared;

namespace Meeting.Application.Members.Queries.GetMemberById;

public sealed class GetMemberByIdQueryHandler : IQueryHandler<GetMemberByIdQuery, MemberResponse>
{
    private readonly IMemberRepository _memberRepository;

    public GetMemberByIdQueryHandler(IMemberRepository memberRepository) => _memberRepository = memberRepository;

    public async Task<Result<MemberResponse>> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken);

        if (member is null)
        {
            return Result.Failure<MemberResponse>(new Error(
                "Member.NotFound",
                $"The member with Id {request.MemberId} was not found"));
        }

        var response = new MemberResponse(member.Id, member.Email.Value);

        return response;
    }
}
//a35d0f7e-48e2-4337-903b-684b80c5e135

