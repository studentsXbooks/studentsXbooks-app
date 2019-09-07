using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using sXb_service.Models;
using sXb_service.EF;
using Microsoft.AspNetCore.Identity;


namespace sXb_service.SampleData
{
    public static class SampleData
    {

        
        public static User MakeUser(string first, string last)
        {
            string emailSuffix = "@wvup.edu";
            User user = new User()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                FirstName = first, LastName = last,
                Email = first.Substring(0, 1).ToLower() + last.ToLower() + emailSuffix,
                NormalizedEmail = first.Substring(0, 1).ToUpper() + last.ToUpper() + emailSuffix.ToUpper(),
                UserName = first.Substring(0,1).ToLower() + last.ToLower(),
                NormalizedUserName = first.Substring(0, 1).ToUpper() + last.ToUpper(),
                EmailConfirmed = true
            };
            user.PicturePath = "random guy.jpeg";
            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, "Test#1");
            //user.RSSFeeds = (List<Rss>)GetRssFeeds(user);
            return user;
        }
        
        public static IEnumerable<User> GetUsers() 
        {
            List<User> users = new List<User>();

            users.Add(MakeUser("John", "Doe"));
            users.Add(MakeUser("Jane", "Jones"));
            users.Add(MakeUser("Joe", "Schmoe"));
            users.Add(MakeUser("James", "Starkey"));
            users.Add(MakeUser("Robert", "Philips"));
            users.Add(MakeUser("Lilly", "Abrams"));
            users.Add(MakeUser("John", "Little"));
            users.Add(MakeUser("Melissa", "Castle"));
            users.Add(MakeUser("James", "Lyon"));
            users.Add(MakeUser("Mandy", "Quest"));
            users.Add(MakeUser("Jane", "Jefferson"));
            users.Add(MakeUser("Don", "Hill"));
            users.Add(MakeUser("Derrick", "Long"));
            users.Add(MakeUser("Rasalee", "Porath"));
            users.Add(MakeUser("Loraine", "Heinemann"));
            users.Add(MakeUser("Harley", "Hawke"));
            users.Add(MakeUser("Larry", "Durrett"));
            users.Add(MakeUser("Hong", "Dumont"));
            users.Add(MakeUser("Alycia", "Aguilar"));
            users.Add(MakeUser("Breann", "Dales"));
            users.Add(MakeUser("Kenia", "Lilly"));
            users.Add(MakeUser("Violeta", "Seiber"));
            users.Add(MakeUser("Kyla", "Pinion"));
            users.Add(MakeUser("Susanne", "Fleurant"));
            users.Add(MakeUser("Joellen", "Sergio"));
            users.Add(MakeUser("Sergio", "Leon"));
            users.Add(MakeUser("Milan", "Marotta"));
            users.Add(MakeUser("Maribel", "Sloan"));
            users.Add(MakeUser("Layla", "Jeffery"));
            users.Add(MakeUser("Burt", "Uribe"));
            users.Add(MakeUser("Cleo", "Brice"));
            users.Add(MakeUser("Noel", "Faver"));
            users.Add(MakeUser("Audie", "Accardi"));
            users.Add(MakeUser("Moses", "Ship"));
            users.Add(MakeUser("Johnny", "Basic"));

            return users;
        }
        public static string SampleText(string name = "")
        {
            string suffix = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin eleifend turpis facilisis metus commodo scelerisque. Nullam facilisis tortor eu metus ornare, ac finibus elit efficitur. ";
            return name + " " + suffix;
        }

    }
}