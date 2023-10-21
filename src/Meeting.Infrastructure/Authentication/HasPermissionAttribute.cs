using Meeting.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Meeting.Infrastructure.Authentication;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permission permission)
        : base(policy: permission.ToString())
    {
    }
}

