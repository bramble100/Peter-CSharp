using Peter.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Peter.Models.Implementations
{
    /// <summary>
    /// A key-value pair to contain the ISIN by the company name. Key: Company name; Value: ISIN
    /// </summary>
    public class NameToIsin : INameToIsin, IEquatable<NameToIsin>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"></param>
        public NameToIsin(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isin"></param>
        public NameToIsin(string name, string isin) : this(name)
        {
            if (!Validators.Isin.IsValidOrEmpty(isin))
            {
                throw new ArgumentNullException(nameof(isin));
            }

            Isin = isin;
        }

        /// <summary>
        /// Company name.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// ISIN.
        /// </summary>
        public string Isin { get; set; }

        public override bool Equals(object obj) => Equals(obj as NameToIsin);

        public bool Equals(NameToIsin other) =>
            other != null &&
                   Name == other.Name &&
                   Isin == other.Isin;

        public override int GetHashCode()
        {
            var hashCode = 1457120196;
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Isin);
            return hashCode;
        }
    }
}