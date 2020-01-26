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
using FluentValidation.AspNetCore;

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

        public IActionResult Index()
        { 
            return View();
        }
        public async Task<IActionResult> GetBooks()
        {
            var responseTask = await _testService.GetValuesAsync("api/books");
            var result = JsonConvert.DeserializeObject<List<BookViewModel>>(responseTask.ToString());
            return View("Index", result);

        }
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Save(BookViewModel bookViewModel)
        {
            var validator = new BookValidator();
            var results = validator.Validate(bookViewModel);
            results.AddToModelState(ModelState, null);
            if (ModelState.IsValid)
            {
                var result = await _testService.PostAsync("api/books", bookViewModel);
                return RedirectToAction("GetBooks","Home");
            }

            return View("Create");
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
