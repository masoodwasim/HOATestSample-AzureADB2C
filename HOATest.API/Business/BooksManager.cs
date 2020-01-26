using HOATest.API.DataContext;
using HOATest.API.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HOATest.API.Business
{
    public class BooksManager : IBooksManager
    {
        private HOATestDBContext _context;
        public BooksManager(HOATestDBContext context)
        {
            _context = context;
        }
        public BookModel AddNewBook(BookModel bookModel)
        {
            var newID = _context.BooksDbSet.Select(x => x.ID).Max() + 1;
            bookModel.ID = newID;
            try
            {
                _context.BooksDbSet.Add(bookModel);
                _context.SaveChanges();
                return bookModel;
            }
            catch (Exception ex)
            {
                // Log exception
                return bookModel;
            }
        }

        public IEnumerable<BookModel> GetLatestBooks()
        {
            var books = _context.BooksDbSet.ToList();
            return books;
        }
    }
}
