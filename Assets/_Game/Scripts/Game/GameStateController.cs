using System;
using MLAPI;
using Tofunaut.TofuUnity;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Tofunaut.GridCCG.Game
{
    [Serializable]
    public class GameStateModel : IAppStateModel
    {
        public AssetReference gameBoardAssetReference;
        public GamePlayer.Model localPlayerModel;
    }

    public class GameStateController : AppStateController<GameStateController, GameStateModel>
    {
        public static Blackboard Blackboard => HasInstance ? null : _instance._blackboard;
        
        private Blackboard _blackboard;
        private GameBoard _gameBoard;

        protected override void Awake()
        {
            base.Awake();

            _blackboard = new Blackboard();

#if UNITY_SERVER
            Debug.Log("start server");
            NetworkManager.Singleton.StartServer();
#else
            Debug.Log("start client");
            NetworkManager.Singleton.StartClient();
#endif
        }

        public override async void SetModel(GameStateModel model)
        {
            base.SetModel(model);
            var gameBoardPrefab = await Addressables.LoadAssetAsync<GameObject>(model.gameBoardAssetReference).Task;
            _gameBoard = Instantiate(gameBoardPrefab).GetComponent<GameBoard>();

            IsReady = true;
        }

        public static GamePlayer.Model GetLocalPlayerModel()
        {
            return !HasInstance ? null : _instance.Model.localPlayerModel;
        }
    }
}