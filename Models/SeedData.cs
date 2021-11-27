using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcFrilance.Data;
using Microsoft.AspNetCore.Identity;
using MvcFrilance.Models;

namespace MvcMovie.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = serviceProvider.GetRequiredService<FrilanceDbContext>())
            {
                if (!context.Specialities.Any())
                {
                    context.Specialities.AddRange(
                        new Speciality { Name = "Разработка" },
                        new Speciality { Name = "Тестирование" },
                        new Speciality { Name = "Администрирование" },
                        new Speciality { Name = "Дизайн" },
                        new Speciality { Name = "Контент" },
                        new Speciality { Name = "Маркетинг" },
                        new Speciality { Name = "Разное" }
                    );
                }
                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Tag { Name = "C#" },
                        new Tag { Name = "C++" },
                        new Tag { Name = "C" },
                        new Tag { Name = "Java" },
                        new Tag { Name = "Python" },
                        new Tag { Name = "Javascript" },
                        new Tag { Name = "PHP" },
                        new Tag { Name = "Rust" },
                        new Tag { Name = "Scala" },
                        new Tag { Name = "F#" },
                        new Tag { Name = "Linux" },
                        new Tag { Name = "ML" },
                        new Tag { Name = "Web" },
                        new Tag { Name = "Game Development" },
                        new Tag { Name = "Frontend" },
                        new Tag { Name = "Backend" },
                        new Tag { Name = "UI/UX" },
                        new Tag { Name = "Desktop" }
                    );
                }
                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(
                        new IdentityRole("Frilancer") { NormalizedName = "FRILANCER" },
                        new IdentityRole("Client") { NormalizedName = "CLIENT" },
                        new IdentityRole("Admin") { NormalizedName = "ADMIN" }
                    );
                }
                context.SaveChanges();
            }
        }
    }
}