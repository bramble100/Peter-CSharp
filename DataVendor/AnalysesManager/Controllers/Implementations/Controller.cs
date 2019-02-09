using AnalysesManager.Controllers.Interfaces;
using AnalysesManager.Services.Implementations;
using AnalysesManager.Services.Interfaces;
using NLog;
using System;

namespace AnalysesManager.Controllers.Implementations
{
    public class Controller : IController
    {
        protected readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IService _service;

        public Controller(IService service)
        {
            _service = service;
        }

        public void GenerateAnalyses()
        {
            _logger.Info("*** Generate Analyses ***");

            try
            {
                _service.GenerateAnalyses();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            _logger.Info("*** *** ***");
        }
    }
}
