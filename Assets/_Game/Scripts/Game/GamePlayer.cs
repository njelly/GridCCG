using System;
using System.Linq;
using MLAPI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Tofunaut.GridCCG.Game
{
    [RequireComponent(typeof(NetworkObject))]
    public class GamePlayer : NetworkBehaviour
    {
        private NetworkObject _networkObject;

        private void Awake()
        {
            _networkObject = GetComponent<NetworkObject>();
        }

        private void Start()
        {
            GameStateController.AddPlayer(this);
            GameStateController.Blackboard.Subscribe<NextStateEvent>(GameStateController_OnNextState);
        }

        private void OnDestroy()
        {
            GameStateController.Blackboard?.Unsubscribe<NextStateEvent>(GameStateController_OnNextState);
        }

        private void GameStateController_OnNextState(NextStateEvent e)
        {
            Debug.Log($"from state {e.prevState} to {e.currentState}");
        }
    }
}