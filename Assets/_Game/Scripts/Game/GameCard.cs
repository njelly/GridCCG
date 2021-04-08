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

        private void Initialize(GamePlayer owner, CardInfo cardInfo)
        {
            CardInfo = cardInfo;
            Owner = owner;

            displayNameLabel.text = cardInfo.displayName;
            costLabel.text = cardInfo.cost.ToString();
        }
    }
}