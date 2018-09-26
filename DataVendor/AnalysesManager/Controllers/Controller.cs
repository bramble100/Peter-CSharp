using AnalysesManager.Services;

namespace AnalysesManager.Controllers
{
    public class Controller
    {
        private readonly Service _service;

        public Controller()
        {
            _service = new Service();
        }

        public void GenerateAnalyses() => _service.GenerateAnalyses();
    }
}
