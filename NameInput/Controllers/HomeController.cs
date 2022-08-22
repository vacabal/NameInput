using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NameInput.Enums;
using NameInput.Models;
using NameInput.Models.DTO;
using NameInput.Services.Interfaces;
using System.Diagnostics;

namespace NameInput.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController>? _logger;
        private readonly INameExportService _svc;

        public HomeController(INameExportService svc, ILogger<HomeController> logger)
        {
            _svc = svc;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(FullNameViewModel name)
        {
            SaveNameStatus status = _svc.AddName(new FullNameDto() { FirstName = name.FirstName, LastName = name.LastName });
            if (status == SaveNameStatus.Success)
            {
                return Ok($"{name.FirstName} {name.LastName} has been saved.");
            }
            else if (status == SaveNameStatus.Duplicate)
            {
                return Ok($"{name.FirstName} {name.LastName} already exists");
            }
            else
            {
                return Ok("An error has occurred");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}