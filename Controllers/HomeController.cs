using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestTask_DS.Models;
using db_context;
using System.Drawing.Printing;
using DatabaseService;

namespace TestTask_DS.Controllers;

public class HomeController : Controller
{
    List<Weather> weather_entrys = new List<Weather> {};
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Data(int id)
    {
        int page = id == 0 ? 1 : id;

        using (var db = new WeatherContext())
        {
            int page_size = 10;
            weather_entrys = db.Weathers.OrderByDescending(w => w.Date).Skip((page - 1) * page_size).Take(page_size).ToList();
        }

        return View(weather_entrys);
    }

    public IActionResult Import()
    {
        return View();
    }

    public IActionResult Upload()
    {
        try
        {
            var request = Request;
            IFormFileCollection files = request.Form.Files;

            if (files == null || files.Count == 0)
            {
                ModelState.AddModelError("", "No files uploaded.");
                return Import();
            }

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            Directory.CreateDirectory(uploadPath);

            foreach (var file in files)
            {
                string fullPath = Path.Combine(uploadPath, file.FileName);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            db_service.XlsxTableImport();
            return RedirectToAction(nameof(Import));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while uploading files.");
            ModelState.AddModelError("", "An error occurred while uploading files.");
            return Import();
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
