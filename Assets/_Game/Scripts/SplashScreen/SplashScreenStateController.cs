using Tofunaut.TofuUnity;

namespace Tofunaut.GridCCG.SplashScreen
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