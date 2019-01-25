namespace Peter.Models.Interfaces
{
    /// <summary>
    /// A key-value pair to contain the ISIN by the company name. Key: Company name; Value: ISIN
    /// </summary>
    public interface INameToIsin
    {
        /// <summary>
        /// Company name.
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// ISIN.
        /// </summary>
        string Isin { get; set; }
    }
}
