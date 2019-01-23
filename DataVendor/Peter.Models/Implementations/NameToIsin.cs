using Peter.Models.Interfaces;

namespace Peter.Models.Implementations
{
    public class NameToIsin : INameToIsin
    {
        private string n;
        private string empty;

        public NameToIsin(string n, string empty)
        {
            this.n = n;
            this.empty = empty;
        }

        public string Name { get; set; }
        public string Isin { get; set; }
    }
}
