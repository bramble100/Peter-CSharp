namespace Infrastructure
{
    /// <summary>
    /// Provides configuration settings.
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// Get config value by key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetValue<T>(string key);
    }
}
