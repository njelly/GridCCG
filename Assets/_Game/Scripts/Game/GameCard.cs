using TMPro;
using Tofunaut.GridCCG.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Tofunaut.GridCCG.Game
{
    public class GameCard : MonoBehaviour
    {
        public CardInfo CardInfo { get; private set; }
        public GamePlayer Owner { get; private set; }

        public TextMeshProUGUI displayNameLabel;
        public TextMeshProUGUI costLabel;

        public Transform statsContainer;
        public TextMeshProUGUI conquerLabel;
        public TextMeshProUGUI powerLabel;
        public TextMeshProUGUI movementLabel;
        public TextMeshProUGUI hitPointsLabel;

        private void Initialize(GamePlayer owner, CardInfo cardInfo)
        {
            CardInfo = cardInfo;
            Owner = owner;

            displayNameLabel.text = cardInfo.displayName;
            costLabel.text = cardInfo.cost.ToString();

            if(cardInfo.spawnUnit)
            {
                conquerLabel.text = cardInfo.spawnUnit.conquer.ToString();
                powerLabel.text = cardInfo.spawnUnit.power.ToString();
                movementLabel.text = cardInfo.spawnUnit.movement.ToString();
                hitPointsLabel.text = cardInfo.spawnUnit.hitPoints.ToString();
            }
            else
            {
                statsContainer.gameObject.SetActive(false);
            }
        }
    }
}