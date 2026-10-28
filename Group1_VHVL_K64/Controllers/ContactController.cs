using System.Diagnostics;
using System.Threading;
using System.Timers;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;
using static System.Formats.Asn1.AsnWriter;

namespace Group1_VHVL_K64.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
            
        }
    }   
}
