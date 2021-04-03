using UnityEngine;

namespace Tofunaut.GridCCG.Game
{
    public class GameBoard : MonoBehaviour
    {
        public Vector2Int boardSize;
        public GameTile gameTilePrefab;
        public float tileSpacing;

        private GameTile[,] _tiles;

        public void Start()
        {
            _tiles = new GameTile[boardSize.x, boardSize.y];
            for (var x = 0; x < _tiles.GetLength(0); x++)
            {
                for (var y = 0; y < _tiles.GetLength(1); y++)
                {
                    _tiles[x, y] = Instantiate(gameTilePrefab, new Vector3(tileSpacing * x, 0f, tileSpacing * y),
                        Quaternion.identity, transform);
                    _tiles[x, y].Initialize(this, new Vector2Int(x, y));
                }
            }
            
            Destroy(gameTilePrefab.gameObject);
        }
    }
}