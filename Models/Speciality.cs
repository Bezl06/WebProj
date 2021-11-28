

namespace MvcFrilance.Models
{
    public class Spell
    {
        public string SpellID { get; set; }
        public List<Order> Orders { get; set; }
        public List<FrilancerAdditionalInfo> Frilancers { get; set; }
    }
    public class Tag
    {
        public string TagID { get; set; }
        public List<Order> Orders { get; set; }
        public List<FrilancerAdditionalInfo> Frilancers { get; set; }
    }
}