using Meeting.Domain.ValueObjects;

namespace Meeting.Persistence.Constants;

public static class CacheKeys
{
    public static Func<Guid, string> MemberById = memberId => $"member-{memberId}";

    public static Func<Email, string> MemberByEmail = email => $"member-email-{email.Value}";
}
