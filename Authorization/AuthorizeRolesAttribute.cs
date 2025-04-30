using Microsoft.AspNetCore.Authorization;

namespace StringManager_API.Authorization;

public class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params string[] roles)
    {
        Roles = string.Join(",", roles);
    }
}