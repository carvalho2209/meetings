using Meeting.Application.Abstractions;
using Meeting.Application.Abstractions.Messaging;
using Meeting.Domain.Errors;
using Meeting.Domain.Repositories;
using Meeting.Domain.Shared;
using Meeting.Domain.ValueObjects;

namespace Meeting.Application.Members.Login;

public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, string>
{
    private readonly IMemberRepository _memberRepository;
    public readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(IMemberRepository memberRepository, IJwtProvider jwtProvider)
    {
        _memberRepository = memberRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Result<Email> email = Email.Create(request.Email);

        var member = await _memberRepository.GetByEmailAsync(email.Value, cancellationToken);

        if (member is null)
        {
            Result.Failure<string>(DomainErrors.Member.InvalidCredentials);
        }

        string token = await _jwtProvider.GenerateAsync(member!);

        return token;
    }
}