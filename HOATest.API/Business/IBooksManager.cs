using HOATest.API.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HOATest.API.Business
{
    public interface IBooksManager
    {
        IEnumerable<BookModel> GetLatestBooks();
        int AddNewBook(BookModel bookModel);
    }
}
