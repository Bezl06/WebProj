

namespace MvcFrilance.Models
{
    public class FrilancerAdditionalInfo
    {
        public int Id { get; set; }
        public string Resume { get; set; }

        public List<Spell> Spells { get; set; } = new();
        public List<Tag> Tags { get; set; } = new();

        public string UserId { get; set; }
        public User User { get; set; }
    }
}