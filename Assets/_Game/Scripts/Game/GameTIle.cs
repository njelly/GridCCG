using UnityEngine;

namespace Tofunaut.GridCCG.Game
{
    public class GameTile : MonoBehaviour
    {
        public Vector2Int Coord { get; private set; }
        
        private GameBoard _gameBoard;
        
        public void Initialize(GameBoard gameBoard, Vector2Int coord)
        {
            _gameBoard = gameBoard;
            Coord = coord;
        }
    }
}