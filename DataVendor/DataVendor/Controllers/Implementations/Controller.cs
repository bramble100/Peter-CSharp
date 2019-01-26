using DataVendor.Controllers.Interfaces;
using DataVendor.Services;
using NLog;
using System;

namespace DataVendor.Controllers.Implementations
{
    public class Controller : IController
    {
        protected readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private IIsinAdderService _isinAdderService;
        private IWebService _webService;

        public IIsinAdderService IsinAdderService
        {
            set => _isinAdderService = value;
        }

        public IWebService WebService
        {
            set => _webService = value;
        }

        public Controller()
        {
            _isinAdderService = new IsinAdderService();
            _webService = new WebService();
        }

        public void WebToCsv()
        {
            _logger.Info("*** Web To Csv ***");

            try
            {
                _webService.Update(_webService.DownloadFromWeb());
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            _logger.Info("*** *** ***");
        }

        public void AddIsins()
        {
            _logger.Info("*** Add Isins ***");

            try
            {
                _isinAdderService.AddIsinsToEntities();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            _logger.Info("*** *** ***");
        }
    }
}
