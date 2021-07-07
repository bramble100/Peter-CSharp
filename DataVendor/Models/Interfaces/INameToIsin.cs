namespace Models.Interfaces
{
    /// <summary>
    /// A key-value pair to contain the ISIN by the company name. Key: Company name; Value: ISIN
    /// </summary>
    public interface INameToIsin
    {
        /// <summary>
        /// Company name.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// ISIN.
        /// </summary>
        string Isin { get; set; }
    }
}
