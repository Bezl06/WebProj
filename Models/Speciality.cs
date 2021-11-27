using System.ComponentModel.DataAnnotations;

namespace MvcFrilance.Models
{
    public class Speciality
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Order> Orders { get; set; }
        public List<FrilancerAdditionalInfo> Frilancers { get; set; }
    }
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Order> Orders { get; set; }
        public List<FrilancerAdditionalInfo> Frilancers { get; set; }
    }
}