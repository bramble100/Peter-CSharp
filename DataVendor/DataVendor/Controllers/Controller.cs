using DataVendor.Services;

namespace DataVendor.Controllers
{
    public class Controller
    {
        internal void WebToCsv()
        {
            var webService = new WebService();
            var latestData = webService.DownloadFromWeb();
            webService.Update(latestData);
        }

        internal void AddIsins()
        {
            var isinAdderService = new IsinAdderService();
            isinAdderService.AddIsinsToEntities();
        }
    }
}
