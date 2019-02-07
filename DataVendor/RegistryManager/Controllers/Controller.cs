using NLog;
using RegistryManager.Services;

namespace RegistryManager.Controllers
{
    public class Controller : IController
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IRegistryService _registryService;

        public Controller(IRegistryService registryService)
        {
            _registryService = registryService;
        }

        public void Update()
        {
            _logger.Info("*** Update Registry ***");

            try
            {
                _registryService.Update();
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex);
            }

            _logger.Info("*** *** ***");
        }
    }
}
