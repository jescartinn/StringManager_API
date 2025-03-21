using Microsoft.AspNetCore.Authorization;

namespace StringManager_API.Authorization;

// Custom authorization attribute to restrict access based on roles
public class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params string[] roles)
    {
        Roles = string.Join(",", roles);
    }
}