using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using sXb_service.EF;
using sXb_service.Models;
using sXb_service.SampleData;
using System.Collections.Generic;

namespace sXb_service.EF
{
    public static class DataInitializer
    {
        
        public static void InitializeData(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<Context>();
            InitializeData(context);
        }
        public static void InitializeData(Context context)
        {
            context.Database.Migrate();
            ClearData(context);
            ResetAllIdentities(context);
            SeedData(context);
        }
        //public static void setTableToNull(Context context)
        //{
        //    var sql = $"Update sXb_service.Follow set UserId = NULL";
        //    context.Database.ExecuteSqlCommand(sql);
        //}
        public static void ClearData(Context context)
        {
            
            DeleteRowsFromTable(context, "dbo", "AspNetUsers");
            
        }
        public static void ResetAllIdentities (Context context)
        {
        }
        public static void DeleteRowsFromTable(Context context, string schemaName, string tableName)
        {
            var sql = $"Delete from [{schemaName}].[{tableName}]";
            context.Database.ExecuteSqlCommand(sql);
        }

        public static void ResetIdentity(Context context)
        {

        }

        public static void ResetIdentity(Context context, string schemaName, string tableName )
        {
            var sql = $"DBCC CHECKIDENT (\"{schemaName}.{tableName}\", RESEED, 0);";
            context.Database.ExecuteSqlCommand(sql);
        }

        public static void SeedData(Context context)
        {
            context.Database.EnsureCreated();

            try
            {
                List<User> users = (List<User>)SampleData.SampleData.GetUsers();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}