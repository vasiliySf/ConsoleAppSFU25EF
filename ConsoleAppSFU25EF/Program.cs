using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ConsoleAppSFU25EF;

internal class Program
{
    static void Main(string[] args)
    {
        // Создаем контекст для добавления данных
        using (var db = new AppContext())
        {
            // Пересоздаем базу
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            // Заполняем данными
            var company1 = new Company { Name = "SF" };
            var company2 = new Company { Name = "VK" };
            var company3 = new Company { Name = "FB" };

            db.Companies.AddRange(company1, company2, company3);

            var user1 = new User { Name = "Arthur", Role = "Admin", Company = company1, Email = "Arthur@gmail.com" };
            var user2 = new User { Name = "Bob", Role = "Admin", Company = company2, Email = "Bob@gmail.com" };
            var user3 = new User { Name = "Clark", Role = "User", Company = company2, Email = "Clark@gmail.com" };
            var user4 = new User { Name = "Dan", Role = "User", Company = company3, Email = "Dan@gmail.com" };

            db.Users.AddRange(user1, user2, user3, user4);

            db.SaveChanges();
        }

        // Создаем контекст для выбора данных
        using (var db = new AppContext())
        {
            var usersQuery =
                from user in db.Users
                where user.CompanyId == 2
                select user;

            var users = db.Users.Include(u => u.Company).Where(u => u.CompanyId == 2);
            foreach (var user in users)
            {
                // Вывод Id пользователей
                Console.WriteLine(user.Id);
            }
            var usersCompany = db.Users.Select(u => u.Company);
            foreach (var user in usersCompany)
            {
                // Вывод Id пользователей
                Console.WriteLine(user.Id);
            }
            var firstUser = db.Users.First();
            Console.WriteLine(firstUser.Id);
           
            var joinedCompany = db.Users.Join(db.Companies,c => c.CompanyId, p => p.Id,  (p, c) => new { companyName =c.Name });
            foreach (var company in joinedCompany)
            {                
                Console.WriteLine(company.companyName);
            }
            var sumCompany = db.Users.Sum(u => u.CompanyId);
            Console.WriteLine(sumCompany);
        }
    }
}