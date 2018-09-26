using RegistryManager.Services;

namespace RegistryManager.Controllers
{
    public class Controller
    {
        internal void Update()
        {
            new Service().Update();
        }
    }
}
