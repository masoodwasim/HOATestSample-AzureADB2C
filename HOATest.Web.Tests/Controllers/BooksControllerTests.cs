using HOATest.API.Business;
using HOATest.API.Controllers;
using HOATest.API.DbModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace HOATest.Web.Tests.Controllers
{
    [TestClass]
    public class BooksControllerTests
    {
        private MockRepository mockRepository;

        private Mock<IBooksManager> mockBooksManager;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Loose);

            this.mockBooksManager = this.mockRepository.Create<IBooksManager>();
        }

        private BooksController CreateBooksController()
        {
            return new BooksController(
                this.mockBooksManager.Object);
        }

        [TestMethod]
        public void GetBooks_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var booksController = this.CreateBooksController();

            // Act
            var result = booksController.GetBooks();

            // Assert
            Assert.AreEqual(result.StatusCode, System.Net.HttpStatusCode.OK);
            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        public void Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var booksController = this.CreateBooksController();
            BookModel book = null;

            // Act
            var result = booksController.Post(
                book);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }
    }
}
