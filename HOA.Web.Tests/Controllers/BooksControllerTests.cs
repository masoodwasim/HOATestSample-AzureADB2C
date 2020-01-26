using HOATest.API.Business;
using HOATest.API.Controllers;
using HOATest.API.DbModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HOA.Web.Tests.Controllers
{
    public class BooksControllerTests
    {
       private readonly BooksController _controller;
        private readonly IBooksManager _service;

        public BooksControllerTests()
        {
            _service = new BooksManagerFake();
            _controller = new BooksController(_service);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = _controller.GetBooks();

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = _controller.GetBooks().Result as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<BookModel>>(okResult.Value);
            Assert.Equal(3, items.Count);
        }

        [Fact]
        public void Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var nameMissingItem = new BookModel()
            {
                Author="Wasim",
                Category="IT-Cloud",
                PublishingCompany="ABC Tech",
                Price = 12.00
            };
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var badResponse = _controller.Post(nameMissingItem);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }


        [Fact]
        public void Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            BookModel testItem = new BookModel()
            {
                Title = "Guinness Original 6 Pack",
                Author = "Guinness",
                Category = "IT-Cloud",
                PublishingCompany = "ABC Tech",
                Price = 50
            };

            // Act
            var createdResponse = _controller.Post(testItem);

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }


        [Fact]
        public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            BookModel testItem = new BookModel()
            {
                Title = "Guinness Original 6 Pack",
                Author = "Guinness",
                Category = "IT-Cloud",
                PublishingCompany = "ABC Tech",
                Price = 50
            };

            // Act
            var createdResponse = _controller.Post(testItem) as CreatedAtActionResult;
            var item = createdResponse.Value as BookModel;

            // Assert
            Assert.IsType<BookModel>(item);
            Assert.Equal("Guinness Original 6 Pack", item.Title);
        }
    }
}
