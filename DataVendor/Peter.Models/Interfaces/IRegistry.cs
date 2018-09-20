using System.Collections.Generic;

namespace Peter.Models.Interfaces
{
    /// <summary>
    /// Dictionary to contain all the shares to analyse. 
    /// Key: ISIN of the stock (ISIN = International Securities Identification Number).
    /// Value: The basic data of the stock.
    /// </summary>
    public interface IRegistry : IDictionary<string, IRegistryEntry>
    {
    }
}
