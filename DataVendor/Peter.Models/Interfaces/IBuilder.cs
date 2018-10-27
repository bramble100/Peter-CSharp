namespace Peter.Models.Interfaces
{
    interface IBuilder<T>
    {
        /// <summary>
        /// Returns the constructed instance.
        /// </summary>
        /// <returns>The constructed instance</returns>
        T Build();
    }
}
