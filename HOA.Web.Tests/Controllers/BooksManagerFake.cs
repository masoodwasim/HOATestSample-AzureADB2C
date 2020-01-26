using HOATest.API.Business;
using HOATest.API.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HOA.Web.Tests.Controllers
{
   public class BooksManagerFake : IBooksManager
    {
        private readonly List<BookModel> _shoppingCart;

        public BooksManagerFake()
        {
            _shoppingCart = new List<BookModel>()
            {
                new BookModel { ID = 1, Title = "Game 1", PublishingCompany = "ABC Tech", 
                    Author = " Author 1", Category = "Category 1", Price = 10 },
                new BookModel { ID = 2, Title = "Game 2", PublishingCompany = "ABC Tech",
                    Author = " Author 2", Category = "Category 2", Price = 20 },
                new BookModel { ID = 3, Title = "Game 3", PublishingCompany = "ABC Tech", 
                    Author = " Author 3", Category = "Category 3", Price = 30 }
            };
        } 
        public IEnumerable<BookModel> GetLatestBooks()
        {
            return _shoppingCart;
        }

        public BookModel AddNewBook(BookModel bookModel)
        { 
            _shoppingCart.Add(bookModel);
            return bookModel;
        }
    }
}
