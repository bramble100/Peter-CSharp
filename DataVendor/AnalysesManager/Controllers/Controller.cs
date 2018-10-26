using AnalysesManager.Services;
using NLog;
using System;

namespace AnalysesManager.Controllers
{
    public class Controller
    {
        protected readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly Service _service;

        public Controller()
        {
            try
            {
                _service = new Service();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public void GenerateAnalyses()
        {
            try
            {
                _service.GenerateAnalyses();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
