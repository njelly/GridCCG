using System.Threading.Tasks;
using Tofunaut.GridCCG.Game;
using UnityEngine;

namespace Tofunaut.GridCCG
{
    public class AppStateMachine : MonoBehaviour
    {
        [Header("Development")]
        public bool skipSplash;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private async void Start()
        {
            await EnterSplash();
        }

        private async Task EnterSplash()
        {
            if (!Debug.isDebugBuild || !skipSplash)
            {
                var splashState = new AppState<SplashScreenStateController, SplashScreenStateModel>(AppConsts.Scenes.SplashScreen);
                await splashState.Enter();
                while(!splashState.IsComplete)
                    await Task.Yield();
            }

            await EnterStart();
        }

        private async Task EnterStart()
        {
            var startScreenState = new AppState<StartScreenStateController, StartScreenStateModel>(AppConsts.Scenes.StartScreen);
            await startScreenState.Enter();
            while(!startScreenState.IsComplete)
                await Task.Yield();

            await EnterGame();
        }

        private async Task EnterGame()
        {
            var inGameState = new AppState<GameStateController, GameStateModel>(AppConsts.Scenes.Game);
            await inGameState.Enter();
            while (!inGameState.IsComplete)
                await Task.Yield();

            await EnterStart();
        }
    }
}