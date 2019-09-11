using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace sXb_service.EF
{
    public class DbInitializer
    {
        private TxtXContext _context;

        public DbInitializer(TxtXContext context)
        {
            _context = context;
        }

        public static void InitializeData(TxtXContext context)
        {
            context.Database.Migrate();
            ClearData(context);
            SeedData(context);
        }

        public static void ClearData(TxtXContext context)
        {
            context.Database.ExecuteSqlCommand("DELETE FROM [dbo].[Books]");
            context.Database.ExecuteSqlCommand("DELETE FROM [dbo].[UserBooks]");
            context.Database.ExecuteSqlCommand("DELETE FROM [dbo].[Listings]");
            context.Database.ExecuteSqlCommand("DELETE FROM [dbo].[AspNetUsers]");
        }

        private static void SeedData(TxtXContext context)
        {
            string books = File.ReadAllText(@"./SampleData/dbo.Books.data.sql");
            string userBooks = File.ReadAllText(@"./SampleData/dbo.UserBooks.data.sql");
            string listings = File.ReadAllText(@"./SampleData/dbo.Listings.data.sql");
            string users = File.ReadAllText(@"./SampleData/dbo.AspNetUsers.data.sql");


            context.Database.ExecuteSqlCommand(books);
            context.Database.ExecuteSqlCommand(userBooks);
            context.Database.ExecuteSqlCommand(listings);
            context.Database.ExecuteSqlCommand(users);
        }
    }
}
