using HOATest.API.Business;
using HOATest.API.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

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
        public ActionResult<IEnumerable<BookModel>> GetBooks()
        {
            var items = _bookManager.GetLatestBooks();
            return Ok(items);
        }
        [HttpPost]
        public IActionResult Post( BookModel book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } 
            var item = _bookManager.AddNewBook(book);
            return CreatedAtAction("Get", new { id = item.ID }, item);

           
        }
    }
}