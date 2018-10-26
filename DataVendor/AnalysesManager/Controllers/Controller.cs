using AnalysesManager.Services;
using NLog;
using System;

namespace AnalysesManager.Controllers
{
    public class Controller
    {
        protected readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        public void GenerateAnalyses()
        {
            _logger.Info("*** Generate Analyses ***");

            try
            {
                new Service().GenerateAnalyses();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            _logger.Info("*** *** ***");
        }
    }
}
