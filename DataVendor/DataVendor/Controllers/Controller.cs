using DataVendor.Services;
using NLog;
using System;

namespace DataVendor.Controllers
{
    public class Controller
    {
        protected readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        internal void WebToCsv()
        {
            _logger.Info("*** Web To Csv ***");

            try
            {
                var webService = new WebService();
                webService.Update(webService.DownloadFromWeb());
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            _logger.Info("*** *** ***");
        }

        internal void AddIsins()
        {
            _logger.Info("*** Add Isins ***");

            try
            {
                new IsinAdderService().AddIsinsToEntities();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            _logger.Info("*** *** ***");
        }
    }
}
