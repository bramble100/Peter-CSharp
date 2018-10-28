using NLog;
using RegistryManager.Services;

namespace RegistryManager.Controllers
{
    public class Controller
    {
        protected readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        internal void Update()
        {
            _logger.Info("*** Update Registry ***");

            try
            {
                new Service().Update();
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex);
            }

            _logger.Info("*** *** ***");
        }
    }
}
