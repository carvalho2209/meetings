using Meeting.Application.Abstractions.Messaging;
using Meeting.Domain.Repositories;
using Meeting.Domain.Shared;

namespace Meeting.Application.Members.Queries.GetMemberById;

public sealed class GetMember : IQuery<MemberVm[]>
{
    public class GetMemberHandler(IMemberRepository memberRepository) : IQueryHandler<GetMember, MemberVm[]>
    {
        public async Task<Result<MemberVm[]>> Handle(GetMember request, CancellationToken cancellationToken)
        {
            var member = await memberRepository.GetAllMembers(cancellationToken);

            if (member is null)
            {
                return Result.Failure<MemberVm[]>(new Error(
                    "Member.NotFound",
                    $"No member was found"));
            }

            var response =
                member
                    .Select(x => new MemberVm(x.Id, x.Email.Value, x.FirstName.Value, x.LastName.Value))
                    .ToArray();

            return response;
        }
    }
}