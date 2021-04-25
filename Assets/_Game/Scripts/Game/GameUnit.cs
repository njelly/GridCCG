using UnityEngine;

namespace Tofunaut.GridCCG.Game
{
    public class GameUnit : MonoBehaviour
    {
        public Vector2Int Coord => new Vector2Int(
            Mathf.RoundToInt(_t.position.x / GameStateController.GameBoard.tileSpacing), 
            Mathf.RoundToInt(_t.position.z / GameStateController.GameBoard.tileSpacing));

        public GamePlayer Owner { get; private set; }

        private Transform _t;

        private void Awake()
        {
            _t = GetComponent<Transform>();
        }

        public void Initialize(GamePlayer owner)
        {
            Owner = owner;
        }
    }
}