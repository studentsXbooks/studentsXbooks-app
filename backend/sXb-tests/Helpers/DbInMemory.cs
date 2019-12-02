using Microsoft.EntityFrameworkCore;
using sXb_service.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace sXb_tests.Helpers
{
    public class DbInMemory
    {
        public static DbContextOptions getDbInMemoryOptions(string dbName)
        {
            var options = new DbContextOptionsBuilder<TxtXContext>()
                  .UseInMemoryDatabase(databaseName: dbName)
                  .Options;
            return options;
        }
    }
}
