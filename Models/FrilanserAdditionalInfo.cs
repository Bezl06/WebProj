

namespace MvcFrilance.Models
{
    public class FrilancerAdditionalInfo
    {
        public int Id { get; set; }
        public string Resume { get; set; }

        public List<Speciality> Specialities { get; set; }
        public List<Tag> Tags { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}