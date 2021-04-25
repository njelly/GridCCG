using Tofunaut.GridCCG.Game;
using UnityEngine;

namespace Tofunaut.GridCCG.Data
{
    [CreateAssetMenu(fileName = "new UnitInfo", menuName = "GridCCG/UnitInfo")]
    public class UnitInfo : ScriptableObject
    {
        public string displayName;
        public GameUnit unitPrefab;
        public int conquer;
        public int power;
        public int movement;
        public int hitPoints;
    }
}