using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MvcFrilance.Models
{
    public class User : IdentityUser
    {
        public string Login { get; set; }
        public List<Order> Orders { get; set; }
        public List<Review> Reviews { get; set; }
        public int FinishedOrders { get; set; }
        public int PositiveRatings { get; set; }
        public int NegativeRatings { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime LastOnline { get; set; }
    }
    public class Review
    {
        public int Id { get; set; }
        public string CreaterUserId { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}