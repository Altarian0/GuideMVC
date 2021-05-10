using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GuideMVC_.Models;
using Microsoft.EntityFrameworkCore;

namespace GuideMVC_.Controllers
{
    public class HomeController : Controller
    {
        private GuideDBContext _db;

        public HomeController(GuideDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var persons = _db.Persons.Include(n=>n.ApplicationUser).ToList();
            return View(persons);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
