﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HOATest.API.Business;
using HOATest.API.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HOATest.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBooksManager _bookManager;
        public BooksController(IBooksManager bookManager)
        {
            _bookManager = bookManager;
        }

        [HttpGet]
        // [Authorize]
        public IActionResult GetBooks()
        {
            var response = _bookManager.GetLatestBooks();
            return Ok(response);
        }
        [HttpPost]
        public IActionResult Post(BookModel book)
        {
            var result = _bookManager.AddNewBook(book);
            if (result > 0)
                return Ok(HttpStatusCode.OK);
            else
                return Ok(HttpStatusCode.BadRequest);
        }
    }
}