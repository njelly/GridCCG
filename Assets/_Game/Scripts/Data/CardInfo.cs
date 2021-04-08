using UnityEngine;

namespace Tofunaut.GridCCG.Data
{
    [CreateAssetMenu(fileName = "new CardInfo", menuName = "GridCCG/CardInfo")]
    public class CardInfo : ScriptableObject
    {
        public string displayName;
        public int cost;
        public UnitInfo spawnUnit;
    }
}