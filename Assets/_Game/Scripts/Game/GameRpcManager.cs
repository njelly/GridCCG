using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

namespace Tofunaut.GridCCG.Game
{
    [RequireComponent(typeof(NetworkObject))]
    public class GameRpcManager : NetworkBehaviour
    {
        [ServerRpc(RequireOwnership = false)]
        public void RequestGameActionFromServerRpc(GameAction.ActionType actionType, string serializedGameAction, ServerRpcParams serverRpcParams = default)
        {
            if (!IsServer)
                return;

            var gameAction = GameAction.Deserialize(actionType, serializedGameAction);

            // the server validates the game action, prints an error message if the action fails, or sends the valid action back to the clients
            if (!gameAction.IsValid)
            {
                var printErrorAction = new PrintErrorMessageAction
                {
                    message = $"GameAction of type {gameAction.Type} is not valid",
                };
                ReceivedGameActionFromServerClientRpc(GameAction.ActionType.PrintErrorMessage, JsonUtility.ToJson(printErrorAction));
            }
            else
                ReceivedGameActionFromServerClientRpc(actionType, serializedGameAction);
        }

        [ClientRpc]
        private void ReceivedGameActionFromServerClientRpc(GameAction.ActionType actionType, string serializedGameAction)
        {
            var gameAction = GameAction.Deserialize(actionType, serializedGameAction);
            gameAction.Execute();
        }
    }
}