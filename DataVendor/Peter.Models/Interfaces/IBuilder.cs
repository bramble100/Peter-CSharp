namespace Peter.Models.Interfaces
{
    interface IBuilder<T>
    {
        /// <summary>
        /// Returns the constructed instance but only when every necessary property set (otherwise null).
        /// </summary>
        /// <returns>The constructed instance</returns>
        T Build();
    }
}
