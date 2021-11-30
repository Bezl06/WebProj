

namespace MvcFrilance.Models
{
    public class Spell
    {
        public string SpellID { get; set; }
        public List<Order> Orders { get; set; } = new();
        public List<FrilancerAdditionalInfo> Frilancers { get; set; } = new();
    }
    public class Tag
    {
        public string TagID { get; set; }
        public List<Order> Orders { get; set; } = new();
        public List<FrilancerAdditionalInfo> Frilancers { get; set; } = new();
    }
}