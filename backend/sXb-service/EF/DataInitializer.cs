using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using sXb_service.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

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
     
        public static void ClearData(Context context)
        {
            
            DeleteRowsFromTable(context, "dbo", "AspNetUsers");
            
        }
        public static void ResetAllIdentities (Context context)
        {
        }
        public async static void DeleteRowsFromTable(Context context, string schemaName, string tableName)
        {
            
            var sql = @"DELETE from [@schemaName].[@tableName]";

            await context.Database.ExecuteSqlCommandAsync(
                sql,
                new SqlParameter("@schemaName", schemaName),
                new SqlParameter("@tableName", tableName)); 
        }

        public static void ResetIdentity(Context context)
        {

        }

        public async static void ResetIdentity(Context context, string schemaName, string tableName )
        {
            var sql = @"DBCC CHECKIDENT (\@schemaName.@tableName\, RESEED, 0);";

            await context.Database.ExecuteSqlCommandAsync(
                sql,
                new SqlParameter("@schemaName", schemaName),
                new SqlParameter("@tableName", tableName));
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