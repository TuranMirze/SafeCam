using Microsoft.AspNetCore.Mvc;
using SafeCam.Models;
using System.Diagnostics;

namespace SafeCam.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Detail()
        {
            return View();
        }
        public IActionResult Service()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
    }
}



