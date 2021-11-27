using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MvcFrilance.Models;

namespace MvcFrilance.Data
{
    public class FrilanceDbContext : IdentityDbContext<User>
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<FrilancerAdditionalInfo> Frilancers { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public FrilanceDbContext(DbContextOptions<FrilanceDbContext> options) : base(options)
        {
        }
    }
}