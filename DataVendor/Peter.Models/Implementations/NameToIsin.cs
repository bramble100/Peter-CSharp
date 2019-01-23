using Peter.Models.Interfaces;

namespace Peter.Models.Implementations
{
    public class NameToIsin : INameToIsin
    {
        public NameToIsin(string name, string isin)
        {
            Name = name;
            Isin = isin;
        }

        public string Name { get; set; }
        public string Isin { get; set; }
    }
}
