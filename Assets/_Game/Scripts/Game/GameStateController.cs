using System;
using Tofunaut.TofuUnity;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Tofunaut.GridCCG.Game
{
    [Serializable]
    public class GameStateModel : IAppStateModel
    {
        public AssetReference gameBoardAssetReference;
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
        }

        public override async void SetModel(GameStateModel model)
        {
            base.SetModel(model);

            var gameBoardPrefab = await Addressables.LoadAssetAsync<GameObject>(model.gameBoardAssetReference).Task;
            _gameBoard = Instantiate(gameBoardPrefab).GetComponent<GameBoard>();

            IsReady = true;
        }
    }
}