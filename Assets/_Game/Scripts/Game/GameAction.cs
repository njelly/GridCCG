using System;
using Tofunaut.GridCCG.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Tofunaut.GridCCG.Game
{
    [Serializable]
    public abstract class GameAction
    {
        public enum ActionType
        {
            Invalid = 0,
            PrintErrorMessage = 1,
            NextGameState = 2,
            PlaceUnit = 3,
        }

        public abstract ActionType Type { get; }
        public virtual bool IsValid => true;

        public abstract void Execute();

        public static GameAction Deserialize(ActionType actionType, string serializedGameAction)
        {
            switch (actionType)
            {
                case ActionType.NextGameState:
                    return JsonUtility.FromJson<NextGameStateAction>(serializedGameAction);
                case ActionType.PlaceUnit:
                    return JsonUtility.FromJson<PlaceUnitAction>(serializedGameAction);
                case ActionType.PrintErrorMessage:
                    return JsonUtility.FromJson<PrintErrorMessageAction>(serializedGameAction);
                default:
                    Debug.LogError($"failed to deserialize GameAction of type {actionType}");
                    return null;
            }
        }
    }

    public class PrintErrorMessageAction : GameAction
    {
        public override ActionType Type => ActionType.PrintErrorMessage;
        public string message;

        public override void Execute()
        {
            Debug.LogError(message);
        }
    }

    public class NextGameStateAction : GameAction
    {
        public override ActionType Type => ActionType.NextGameState;
        public override void Execute()
        {
            GameStateController.NextState();
        }
    }

    public class PlaceUnitAction : GameAction
    {
        public override ActionType Type => ActionType.PlaceUnit;
        public int playerIndex;
        public Vector2Int coord;
        public string unitInfoAssetReferencePath;

        public override async void Execute()
        {
            var unitInfo = await Addressables.LoadAssetAsync<UnitInfo>(unitInfoAssetReferencePath).Task;
            GameStateController.GetPlayer(playerIndex).PlaceUnit(unitInfo, coord);
        }
    }
}