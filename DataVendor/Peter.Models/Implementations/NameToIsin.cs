using Peter.Models.Interfaces;

namespace Peter.Models.Implementations
{
    public class NameToIsin : INameToIsin
    {
        public string Name { get; set; }
        public string Isin { get; set; }
    }
}
