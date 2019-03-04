using NLog;
using System;

namespace Infrastructure
{
    /// <summary>
    /// Provides access to local environment variables.
    /// </summary>
    public class EnvironmentVariableReader : IEnvironmentVariableReader
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Retrieves the value of an environment variable from the current process or from the 
        /// Windows operating system registry key for the current user or local machine.
        /// </summary>
        /// <param name="name">The name of an environment variable.</param>
        /// <returns></returns>
        public string GetEnvironmentVariable(string name)
        {
            try
            {
                return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User);
            }
            catch (Exception ex)
            {
                _logger.Debug(ex);
                throw;
            }
        }
    }
}
