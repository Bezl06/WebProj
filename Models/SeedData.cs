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
                if (!context.Spells.Any())
                {
                    context.Spells.AddRange(
                        new Spell { SpellID = "Разработка" },
                        new Spell { SpellID = "Тестирование" },
                        new Spell { SpellID = "Администрирование" },
                        new Spell { SpellID = "Дизайн" },
                        new Spell { SpellID = "Контент" },
                        new Spell { SpellID = "Маркетинг" },
                        new Spell { SpellID = "Разное" }
                    );
                }
                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Tag { TagID = "C#" },
                        new Tag { TagID = "C++" },
                        new Tag { TagID = "C" },
                        new Tag { TagID = "Java" },
                        new Tag { TagID = "Python" },
                        new Tag { TagID = "Javascript" },
                        new Tag { TagID = "PHP" },
                        new Tag { TagID = "Rust" },
                        new Tag { TagID = "Scala" },
                        new Tag { TagID = "F#" },
                        new Tag { TagID = "Linux" },
                        new Tag { TagID = "ML" },
                        new Tag { TagID = "Web" },
                        new Tag { TagID = "Game Development" },
                        new Tag { TagID = "Frontend" },
                        new Tag { TagID = "Backend" },
                        new Tag { TagID = "UI/UX" },
                        new Tag { TagID = "Desktop" }
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