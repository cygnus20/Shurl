using System.Security.Claims;

namespace Shurl.Core;

public class GetUserClaims(IHttpContextAccessor accessor) : IGetUserClaims
{
    public string? UserId => accessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
}
