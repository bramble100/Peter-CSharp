using NLog;
using RegistryManager.Services;

namespace RegistryManager.Controllers
{
    public class Controller
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IRegistryService _registryService;

        public Controller()
        {
            _registryService = new RegistryService();
        }

        public Controller(IRegistryService registryService) : this()
        {
            _registryService = registryService;
        }

        internal void Update()
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
