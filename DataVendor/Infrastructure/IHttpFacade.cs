using System.Net.Http;

namespace Infrastructure
{
    /// <summary>
    /// Provides access to the Web via HTTP.
    /// </summary>
    public interface IHttpFacade
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClient"/> class.
        /// </summary>
        /// <returns></returns>
        HttpClient GetHttpClient();
    }
}
