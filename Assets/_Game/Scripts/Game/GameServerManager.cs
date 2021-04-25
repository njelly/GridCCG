using UnityEngine;

namespace Tofunaut.GridCCG.Game
{
    public class GameServerManager : MonoBehaviour
    {
        private void Start()
        {
            GameStateController.Blackboard.Subscribe<PlayerAddedEvent>(OnPlayerAdded);
        }

        private void OnDestroy()
        {
            GameStateController.Blackboard.Unsubscribe<PlayerAddedEvent>(OnPlayerAdded);
        }

        private void OnPlayerAdded(PlayerAddedEvent e)
        {

        }
    }
}