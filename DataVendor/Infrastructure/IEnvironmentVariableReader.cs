namespace Infrastructure
{
    /// <summary>
    /// Provides access to local environment variables.
    /// </summary>
    public interface IEnvironmentVariableReader
    {
        /// <summary>
        /// Retrieves the value of an environment variable from the current process or from the 
        /// Windows operating system registry key for the current user or local machine.
        /// </summary>
        /// <param name="name">The name of an environment variable.</param>
        /// <returns></returns>
        string GetEnvironmentVariable(string name);
    }
}
