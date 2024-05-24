using Logis.AspNet.DAL;
using Logis.AspNet.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Logis.AspNet.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Service> services = _context.services.ToList();
            return View(services);
        }

        
    }
}
