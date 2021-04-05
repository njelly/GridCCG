using UnityEngine;

namespace Tofunaut.GridCCG.Data
{
    [CreateAssetMenu(fileName = "new CardInfo", menuName = "GridCCG/CardInfo")]
    public class CardInfo : ScriptableObject
    {
        public UnitInfo SpawnUnit;
    }
}