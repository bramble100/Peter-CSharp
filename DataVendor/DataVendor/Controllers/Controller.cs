using DataVendor.Services;

namespace DataVendor.Controllers
{
    public class Controller
    {
        internal void WebToCsv()
        {
            var webService = new WebService();
            webService.Update(webService.DownloadFromWeb());
        }

        internal void AddIsins() => new IsinAdderService().AddIsinsToEntities();
    }
}
