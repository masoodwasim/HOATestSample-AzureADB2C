using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HOATest.Web.Models;
using HOATest.Web.Proxy;
using Newtonsoft.Json;
using AutoMapper;
 

namespace HOATest.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TestServiceProxy _testService;

        public HomeController(ILogger<HomeController> logger, TestServiceProxy testService)
        {
            _logger = logger;
            _testService = testService;
        }

        public async Task<IActionResult> Index()
        { 
            if (User.Identity.IsAuthenticated)
            { 
                var responseTask = await _testService.GetValuesAsync("api/books"); 
                var result = JsonConvert.DeserializeObject<List<BookViewModel>>(responseTask.ToString());  
                return View(result);
            }
            else
            {
                return View();
            }
          
        }
        public async Task<IActionResult> GetBooks()
        {
            if (User.Identity.IsAuthenticated)
            {
                var responseTask = await _testService.GetValuesAsync("api/books");
                var result = JsonConvert.DeserializeObject<List<BookViewModel>>(responseTask.ToString());
                return View(result);
            }
            else
            {
                return View();
            }

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
