using HOATest.API.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HOATest.API.DataContext
{
    public class HOATestDBContext : DbContext
    {
        public HOATestDBContext(DbContextOptions<HOATestDBContext> options)
           : base(options)
        {
        }
        public DbSet<BookModel> BooksDbSet { get; set; }
    }
}
