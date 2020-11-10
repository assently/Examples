using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Assently.Samples.DotnetCore.Controllers
{
    [Authorize]
    public class ProtectedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}