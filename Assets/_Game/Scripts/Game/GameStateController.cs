using System;
using System.Collections.Generic;
using System.Linq;
using MLAPI;
using Tofunaut.TofuUnity;
using Tofunaut.TofuUnity.Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Tofunaut.GridCCG.Game
{
    [Serializable]
    public class GameStateModel : IAppStateModel
    {
        public AssetReference gameBoardAssetReference;
        public int expectedPlayers;
    }

    public class GameStateController : AppStateController<GameStateController, GameStateModel>
    {
        public enum GameState
        {
            Invalid,
            WaitingForPlayers,
            InGame,
            PostGame,
        }

        public static Blackboard Blackboard => HasInstance ? null : _instance._blackboard;
        public static GameState CurrentState => HasInstance ? _instance._gameState : GameState.Invalid;
        
        private Blackboard _blackboard;
        private GameBoard _gameBoard;
        private GameState _gameState;
        private List<GamePlayer> _gamePlayers;

        protected override void Awake()
        {
            base.Awake();

            _blackboard = new Blackboard();
            _gamePlayers = new List<GamePlayer>();
            _gameState = GameState.Invalid;

#if UNITY_SERVER
            Debug.Log("start server");
            NetworkManager.Singleton.StartServer();
#else
            Debug.Log("start client");
            NetworkManager.Singleton.StartClient();
#endif
            
            NextState();
        }

        public override async void SetModel(GameStateModel model)
        {
            base.SetModel(model);
            var gameBoardPrefab = await Addressables.LoadAssetAsync<GameObject>(model.gameBoardAssetReference).Task;
            _gameBoard = Instantiate(gameBoardPrefab).GetComponent<GameBoard>();

            IsReady = true;
        }

        private void NextState()
        {
            var gameStateInt = (int) _gameState;
            gameStateInt++;

            var allGameStates = (GameState[])Enum.GetValues(typeof(GameState));
            if (gameStateInt < 0 || gameStateInt > allGameStates.Length - 1)
                return;

            var prevState = _gameState;
            _gameState = allGameStates[gameStateInt];
            
            Blackboard.Invoke(new NextStateEvent(prevState, _gameState));
        }

        public static void AddPlayer(GamePlayer player)
        {
            if (!HasInstance)
            {
                Debug.LogError("failed to add player, no GameStateController instance exists");
                return;
            }

            if (CurrentState != GameState.WaitingForPlayers)
            {
                Debug.LogError("failed to add player, not currently waiting for players");
                return;
            }

            if (_instance._gamePlayers.Contains(player))
            {
                Debug.LogError("failed to add player, player has already been added");
                return;
            }
            
            _instance._gamePlayers.Add(player);

            if (_instance._gamePlayers.Count >= _instance.Model.expectedPlayers)
                _instance.NextState();
        }
    }

    public class NextStateEvent : IBlackboardEvent
    {
        public readonly GameStateController.GameState prevState;
        public readonly GameStateController.GameState currentState;

        public NextStateEvent(GameStateController.GameState prevState, GameStateController.GameState currentState)
        {
            this.prevState = prevState;
            this.currentState = currentState;
        }
    }
}