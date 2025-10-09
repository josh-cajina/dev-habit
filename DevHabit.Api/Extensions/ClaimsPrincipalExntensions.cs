using System.Security.Claims;

namespace DevHabit.Api.Extensions;

public static class ClaimsPrincipalExntensions
{
    public static string? GetIdentityId(this ClaimsPrincipal? principal) 
    {
        string? identityId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return identityId;
    }
}
