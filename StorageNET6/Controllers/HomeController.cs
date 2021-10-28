using Microsoft.AspNetCore.Mvc;
using Storage.Models;

namespace Storage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Logical _logical;

        public HomeController(ILogger<HomeController> logger, Logical logical)
        {
            _logger = logger;
            _logical = logical;

        }

        public async Task<IActionResult> Index() => View(await _logical.GetImages());

        [HttpGet]
        public IActionResult Add() => View();
        [HttpPost]
        public async Task<IActionResult> Add(IFormFile image)
        {
            if (!await _logical.AddImage(image.FileName, ImageToByteArray(image)))
            {
                return View();
            }
            return RedirectToAction("Index");
        }

        public byte[] ImageToByteArray(IFormFile image)
        {
            using (var ms = new MemoryStream())
            {
                image.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
