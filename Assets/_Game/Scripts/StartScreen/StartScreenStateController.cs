using Tofunaut.TofuUnity;

namespace Tofunaut.GridCCG.StartScreen
{
    public class StartScreenStateModel : IAppStateModel
    {
        
    }
    
    public class StartScreenStateController : AppStateController<StartScreenStateController, StartScreenStateModel>
    {
        private void Start()
        {
            IsReady = true;
        }
    }
}