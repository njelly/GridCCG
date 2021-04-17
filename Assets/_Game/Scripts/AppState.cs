using System.Threading.Tasks;
using Tofunaut.TofuUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tofunaut.GridCCG
{
    public class AppState<T1, T2> where T1 : AppStateController<T1, T2> where T2 : IAppStateModel
    {
        public bool IsComplete => _stateController && _stateController.IsReady && _stateController.IsComplete;

        private readonly int _sceneIndex;

        private T1 _stateController;

        public AppState(int sceneIndex)
        {
            _sceneIndex = sceneIndex;
        }

        public virtual async Task Enter(T2 model)
        {
            var loadTask = SceneManager.LoadSceneAsync(_sceneIndex);
            while (!loadTask.isDone)
                await Task.Yield();

            _stateController = Object.FindObjectOfType<T1>();
            if (!_stateController)
            {
                Debug.LogError($"no object found of type {nameof(T1)}");
                return;
            }
            
            _stateController.SetModel(model);
        }
    }
}