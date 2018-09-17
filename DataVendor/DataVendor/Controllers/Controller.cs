using DataVendor.Services;

namespace DataVendor.Controllers
{
    public class Controller
    {
        private readonly WebService _webService;

        public Controller()
        {
            _webService = new WebService();
        }

        public void WebToCsv()
        {
            var latestData = _webService.DownloadFromWeb();
            _webService.Update(latestData);
        }
    }
}
