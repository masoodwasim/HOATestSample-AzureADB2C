using HOATest.API.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HOATest.API.DataContext
{
    public class BooksDataSeed
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new HOATestDBContext(
                serviceProvider.GetRequiredService<DbContextOptions<HOATestDBContext>>()))
            {
                // Look for any books already in database.
                if (context.BooksDbSet.Any())
                {
                    return;   // Database has been seeded
                }

                context.BooksDbSet.AddRange(
                    new BookModel
                    {
                        ID = 1,
                        Title = "Azure for Architects",
                        PublishingCompany = "Microsoft",
                        Author="John Savill",
                        Category="IT-Cloud",
                        Price=50.00
                    },
                    new BookModel
                    {
                        ID = 2,
                        Title = "Microsoft Visual C# Step By Step",
                        PublishingCompany = "Microsoft",
                        Author = "John Sharp",
                        Category = "IT-Programming",
                        Price = 55.00
                    },
                    new BookModel
                    {
                        ID = 3,
                        Title = "Step by Step Javascript",
                        PublishingCompany = "Microsoft",
                        Author = "Steve Suehring",
                        Category = "IT-Programming",
                        Price = 35.50
                    },
                    new BookModel
                    {
                        ID = 4,
                        Title = "ASP.NET Core 3.0",
                        PublishingCompany = "Microsoft",
                        Author = "John Savill",
                        Category = "IT-Programming",
                        Price = 65.00
                    },
                    new BookModel
                    {
                        ID = 5,
                        Title = "Azure for Developers",
                        PublishingCompany = "Microsoft",
                        Author = "Kamil Mrzyglod",
                        Category = "IT-Cloud",
                        Price = 70.50
                    },
                    new BookModel
                    {
                        ID = 6,
                        Title = "C# in depth",
                        PublishingCompany = "DreamTech",
                        Author = "Jon Skeet, Eric Lippert",
                        Category = "IT-Programming",
                        Price = 50.50
                    });

                context.SaveChanges();
            }
        }
    }
}
