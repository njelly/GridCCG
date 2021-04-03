using Tofunaut.TofuUnity;

namespace Tofunaut.GridCCG
{    public class SplashScreenStateModel : IAppStateModel
    {
        
    }
    
    public class SplashScreenStateController : AppStateController<SplashScreenStateController, SplashScreenStateModel>
    {
        private void Start()
        {
            IsReady = true;
        }
    }
}