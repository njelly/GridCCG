using System.Threading.Tasks;
using Tofunaut.GridCCG.Game;
using Tofunaut.GridCCG.SplashScreen;
using Tofunaut.GridCCG.StartScreen;
using UnityEngine;

namespace Tofunaut.GridCCG
{
    public class AppStateMachine : MonoBehaviour
    {
        [Header("Development")]
        public GameStateModel debugGameStateModel;

        private SplashScreenStateModel _splashScreenStateModel;
        private StartScreenStateModel _startScreenStateModel;
        private GameStateModel _gameStateModel;
        private bool _isServer;

        private void Awake()
        {
#if UNITY_SERVER
            _isServer = true;
#endif
            
            DontDestroyOnLoad(gameObject);
        }

        private async void Start()
        {
            _splashScreenStateModel = default;
            _startScreenStateModel = default;
            _gameStateModel = Debug.isDebugBuild ? debugGameStateModel : default;

            if (_isServer)
                await EnterGame();
            else
                await EnterSplash();
        }

        private async Task EnterSplash()
        {
            var splashState = new AppState<SplashScreenStateController, SplashScreenStateModel>(AppConsts.Scenes.SplashScreen);
            await splashState.Enter(_splashScreenStateModel);
            while(!splashState.IsComplete)
                await Task.Yield();

            await EnterStart();
        }

        private async Task EnterStart()
        {
            var startScreenState =
                new AppState<StartScreenStateController, StartScreenStateModel>(AppConsts.Scenes.StartScreen);
            await startScreenState.Enter(_startScreenStateModel);
            while (!startScreenState.IsComplete)
                await Task.Yield();

            await EnterGame();
        }

        private async Task EnterGame()
        {
            var inGameState = new AppState<GameStateController, GameStateModel>(AppConsts.Scenes.Game);
            await inGameState.Enter(_gameStateModel);
            while (!inGameState.IsComplete)
                await Task.Yield();

            await EnterStart();
        }
    }
}