using Logis.AspNet.DAL;
using Logis.AspNet.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

namespace Logis.AspNet.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ServiceController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        AppDbContext _context;
        public ServiceController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context= context;
            _environment = environment;
        }
        public IActionResult Index()
        {
            List<Service> services = _context.services.ToList();
            return View(services);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Service service)
        {
            if (!service.ImgFile.ContentType.Contains("image/"))
            {
                return View();
            }
            string path = _environment.WebRootPath + @"\Upload\";
            string filename = Guid.NewGuid() + service.ImgFile.FileName;
            using (FileStream stream = new FileStream(path + filename, FileMode.Create))
            {
                service.ImgFile.CopyTo(stream);
            }
            service.ImgUrl = filename;
            _context.services.Add(service);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            var service= _context.services.FirstOrDefault(s => s.Id == id);
            if (service == null)
            {
                return View();
            }
            return View(service);
        }
        [HttpPost]
        public IActionResult Update(Service newservice)
        {
            var oldservice= _context.services.FirstOrDefault(x=>x.Id == newservice.Id);
            if (oldservice == null)
            {
                return View();
            }
            if (newservice.ImgFile != null)
            {
                string path = _environment.WebRootPath + @"\Upload\";
                FileInfo fileInfo = new FileInfo(path+oldservice.ImgUrl);
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
                string filename = Guid.NewGuid() + newservice.ImgFile.FileName;
                using (FileStream stream = new FileStream(path + filename, FileMode.Create))
                {
                    newservice.ImgFile.CopyTo(stream);
                }
                oldservice.ImgUrl = filename;
            }
            oldservice.Name = newservice.Name;
            oldservice.Description = newservice.Description;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var service = _context.services.FirstOrDefault(x=>x.Id == id);
            if (service != null)
            {
                _context.services.Remove(service);
            _context.SaveChanges();
            return Ok();

                
            }
            return BadRequest();    
        }


    } 
}
