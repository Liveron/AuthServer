using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthServer.Controllers
{
    [Controller]
    [Route("[Controller]/[action]")]
    public class BaseController : ControllerBase
    {
        internal Guid UserId => !User.Identity.IsAuthenticated
            ? Guid.Empty
            : Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    }
}
