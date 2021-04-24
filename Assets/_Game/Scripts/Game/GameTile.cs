using UnityEngine;

namespace Tofunaut.GridCCG.Game
{
    public class GameTile : MonoBehaviour
    {
        public Vector2Int Coord { get; private set; }

        public Material evenMaterial;
        public Material oddMaterial;
        
        private GameBoard _gameBoard;
        
        public void Initialize(GameBoard gameBoard, Vector2Int coord)
        {
            _gameBoard = gameBoard;
            Coord = coord;
            var isEven = (coord.x + coord.y * gameBoard.boardSize.x) % 2 == 0;

            var renderers = GetComponentsInChildren<Renderer>();
            foreach (var r in renderers)
            {
                var mats = r.materials;
                mats[0] = isEven ? Instantiate(evenMaterial) : oddMaterial;
                r.materials = mats;
            }
        }
    }
}