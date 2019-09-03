using Microsoft.EntityFrameworkCore;
using sXb_service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sXb_service.EF
{
    public class TxtXContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public DbSet<Listing> Listings { get; set; }

        public DbSet<UserBook> UserBooks { get; set; }
        
        public TxtXContext()
        {

        }

        public TxtXContext(DbContextOptions options) : base(options)
        {
            try
            {
                Database.Migrate();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
    }
}
