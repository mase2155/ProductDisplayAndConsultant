using Group1_VHVL_K64.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Group1_VHVL_K64.Areas.Admins.Controllers
{
    [Area("Admins")]
    [Authorize(Roles = AppRoles.Admin)]
    public abstract class AdminBaseController : Controller
    {
    }
}
