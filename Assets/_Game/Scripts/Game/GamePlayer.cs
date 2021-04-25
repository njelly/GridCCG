using MLAPI;
using System.Collections.Generic;
using Tofunaut.GridCCG.Data;
using UnityEngine;

namespace Tofunaut.GridCCG.Game
{
    public class GamePlayer : NetworkBehaviour
    {
        public int Index
        {
            get
            {
                if (_playerIndex < 0)
                    _playerIndex = GameStateController.GetPlayerIndex(this);

                return _playerIndex;
            }
        }

        private int _playerIndex;
        private List<GameUnit> _units;

        private void Start()
        {
            _playerIndex = -1;
            GameStateController.AddPlayer(this);
            GameStateController.Blackboard.Subscribe<NextStateEvent>(GameStateController_OnNextState);
            GameStateController.Blackboard.Subscribe<BeginPlayerTurnEvent>(GameStateController_BeginPlayerTurn);
        }

        private void OnDestroy()
        {
            GameStateController.Blackboard?.Unsubscribe<NextStateEvent>(GameStateController_OnNextState);
            GameStateController.Blackboard?.Unsubscribe<BeginPlayerTurnEvent>(GameStateController_BeginPlayerTurn);
        }

        private void GameStateController_OnNextState(NextStateEvent e)
        {
            if(e.currentState == GameStateController.GameState.InGame)
            {
                // place the unit's tower when it the game begins
                GameStateController.RequestGameActionFromServer(new PlaceUnitAction
                {
                    playerIndex = Index,
                    coord = GameStateController.GameBoard.GetTowerPlacementForPlayerIndex(Index),
                    unitInfoAssetReferencePath = "Assets/_Game/Data/Units/TowerUnit.asset",
                });
            }
        }

        public void PlaceUnit(UnitInfo unitInfo, Vector2Int coord)
        {
            _units ??= new List<GameUnit>();
            var pos = new Vector3(coord.x * GameStateController.GameBoard.tileSpacing, 0f, coord.y * GameStateController.GameBoard.tileSpacing);
            var gameUnit = Instantiate(unitInfo.unitPrefab, pos, Quaternion.identity, null);
            _units.Add(gameUnit);
            gameUnit.Initialize(this);

        }

        private void GameStateController_BeginPlayerTurn(BeginPlayerTurnEvent e)
        {
            if (e.player != this)
                return;

            Debug.Log($"begin player {Index}'s turn");
        }
    }
}