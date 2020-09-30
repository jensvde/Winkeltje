using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Winkeltje.Models;

namespace Winkeltje.Controllers
{
    public class HomeController : Controller
    {
        IProductManager _manager = new ProductManager();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(_manager.GetHomeImages().ToList());
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

        public FileContentResult getImg(int id)
        {
            byte[] byteArray = _manager.GetHomeImage(id).ImageData;
            return byteArray != null
                ? new FileContentResult(byteArray, "image/jpeg")
                : null;
        }
    }
}
