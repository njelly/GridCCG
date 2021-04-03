using System.Threading.Tasks;
using Tofunaut.GridCCG.Game;
using UnityEngine;

namespace Tofunaut.GridCCG
{
    public class AppStateMachine : MonoBehaviour
    {
        [Header("Development")]
        public bool skipSplash;
        public bool skipStart;
        public GameStateModel debugGameStateModel;

        private SplashScreenStateModel _splashScreenStateModel;
        private StartScreenStateModel _startScreenStateModel;
        private GameStateModel _gameStateModel;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private async void Start()
        {
            _splashScreenStateModel = default;
            _startScreenStateModel = default;
            _gameStateModel = Debug.isDebugBuild ? debugGameStateModel : default;
            
            await EnterSplash();
        }

        private async Task EnterSplash()
        {
            if (!Debug.isDebugBuild || !skipSplash)
            {
                var splashState = new AppState<SplashScreenStateController, SplashScreenStateModel>(AppConsts.Scenes.SplashScreen);
                await splashState.Enter(_splashScreenStateModel);
                while(!splashState.IsComplete)
                    await Task.Yield();
            }

            await EnterStart();
        }

        private async Task EnterStart()
        {
            if (!Debug.isDebugBuild || !skipStart)
            {
                var startScreenState =
                    new AppState<StartScreenStateController, StartScreenStateModel>(AppConsts.Scenes.StartScreen);
                await startScreenState.Enter(_startScreenStateModel);
                while (!startScreenState.IsComplete)
                    await Task.Yield();
            }

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