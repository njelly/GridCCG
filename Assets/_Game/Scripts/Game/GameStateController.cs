using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Spawning;
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

        public static Blackboard Blackboard => HasInstance ? _instance._blackboard : null;
        public static GameState CurrentState => HasInstance ? _instance._gameState : GameState.Invalid;
        public static int NumPlayers => HasInstance ? _instance._gamePlayers.Count : 0;
        public static int ExpectedPlayers => HasInstance ? _instance.Model.expectedPlayers : 0;
        public static GamePlayer LocalPlayer => HasInstance ? _instance._gamePlayers.First(x => x.IsLocalPlayer) : null;
        public static GameBoard GameBoard => HasInstance ? _instance._gameBoard : null;

        public GameRpcManager gameRpcManagerPrefab;

        private Blackboard _blackboard;
        private GameBoard _gameBoard;
        private GameState _gameState;
        private List<GamePlayer> _gamePlayers;
        private int _currentPlayerTurn;
        private GameRpcManager _gameRpcManager;

        protected override async void Awake()
        {
            base.Awake();

            _blackboard = new Blackboard();
            _gamePlayers = new List<GamePlayer>();
            _gameState = GameState.Invalid;
            _currentPlayerTurn = -1;

            NextState();

#if UNITY_SERVER
            Debug.Log("start server");
            NetworkManager.Singleton.StartServer();
            _gameRpcManager = Instantiate(gameRpcManagerPrefab);
            _gameRpcManager.NetworkObject.Spawn();
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

        public static void NextState()
        {
            var gameStateInt = (int) _instance._gameState;
            gameStateInt++;

            var allGameStates = (GameState[])Enum.GetValues(typeof(GameState));
            if (gameStateInt < 0 || gameStateInt > allGameStates.Length - 1)
                return;

            var prevState = _instance._gameState;
            _instance._gameState = allGameStates[gameStateInt];

            switch (_instance._gameState)
            {
                case GameState.InGame:
                    _instance.OnEnteredState_InGame();
                    break;
            }

            Debug.Log($"entered gamestate: {_instance._gameState}");
            Blackboard.Invoke(new NextStateEvent(prevState, _instance._gameState));
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
            _instance._gamePlayers.Sort((a, b) => a.OwnerClientId.CompareTo(b.OwnerClientId));

            Blackboard.Invoke(new PlayerAddedEvent(player));

            if (_instance._gamePlayers.Count >= _instance.Model.expectedPlayers)
                RequestGameActionFromServer(new NextGameStateAction());
        }

        public static int GetPlayerIndex(GamePlayer player)
        {
            if (_instance._gameState < GameState.InGame)
                return -1;

            for(var i = 0; i < _instance._gamePlayers.Count; i++)
            {
                if (_instance._gamePlayers[i] == player)
                    return i;
            }

            return -1;
        }

        public static GamePlayer GetPlayer(int index)
        {
            if (index < 0 || index >= _instance._gamePlayers.Count)
                return null;

            return _instance._gamePlayers[index];
        }

        private void OnEnteredState_InGame()
        {
            BeginNextPlayerTurn();
        }

        private void BeginNextPlayerTurn()
        {
            _currentPlayerTurn++;
            _currentPlayerTurn %= NumPlayers;
        }

        public static async void RequestGameActionFromServer(GameAction gameAction)
        {
            while (!_instance._gameRpcManager)
            {
                _instance._gameRpcManager = FindObjectOfType<GameRpcManager>();
                await Task.Yield();
            }

            _instance._gameRpcManager.RequestGameActionFromServerRpc(gameAction.Type, JsonUtility.ToJson(gameAction));
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

    public class PlayerAddedEvent : IBlackboardEvent
    {
        public readonly GamePlayer player;

        public PlayerAddedEvent(GamePlayer player)
        {
            this.player = player;
        }
    }

    public class BeginPlayerTurnEvent : IBlackboardEvent
    {
        public readonly GamePlayer player;

        public BeginPlayerTurnEvent(GamePlayer player)
        {
            this.player = player;
        }
    }
}