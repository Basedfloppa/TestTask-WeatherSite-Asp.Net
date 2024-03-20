using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestTask_DS.Models;
using db_context;
using System.Drawing.Printing;
using DatabaseService;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace TestTask_DS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Data(int? year, int? month, int id = 1)
        {
            int page = id < 1 ? 1 : id;
            int pageSize = 10;

            WeatherContext db = new WeatherContext();
            List<Weather> weather_entries;

            if (year.HasValue && month.HasValue)
            {
                int yearValue = year.Value;
                int monthValue = month.Value;
                DateTime startDate = new DateTime(yearValue, monthValue, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                
                List<Weather> weatherQuery = db.Weathers.Where(w => w.Date.Year == yearValue && w.Date.Month == monthValue).ToList();
                weather_entries = weatherQuery
                                .OrderByDescending(w => w.Date)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
            }
            else 
            {
                weather_entries = db.Weathers
                                .OrderByDescending(w => w.Date)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
            }

            

            return View(weather_entries);
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
}