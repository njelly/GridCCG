using System;
using System.Linq;
using MLAPI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Tofunaut.GridCCG.Game
{
    public class GamePlayer : NetworkBehaviour
    {
        public enum Controller
        {
            Local,
            Network,
            AI,
        }

        [Serializable]
        public class Model
        {
            public string displayName;
            public Controller controller;
            public CardCountPair[] cardCountPairs;

            [Serializable]
            public class CardCountPair
            {
                public AssetReference cardInfoReference;
                public int count;
            }

            public NetworkModel ToNetworkModel()
            {
                return new NetworkModel
                {
                    displayName = displayName,
                    controller = controller,
                    cardCountPairs = cardCountPairs.Select(x => new NetworkModel.CardCountPair
                    {
                        count = x.count,
                        assetGuid = x.cardInfoReference.AssetGUID,
                    }).ToArray(),
                };
            }
        }

        [Serializable]
        public class NetworkModel
        {
            public string displayName;
            public Controller controller;
            public CardCountPair[] cardCountPairs;
            
            [Serializable]
            public class CardCountPair
            {
                public string assetGuid;
                public int count;
            }            
            
            public Model ToLocalModel()
            {
                return new Model
                {
                    displayName = displayName,
                    controller = controller,
                    cardCountPairs = cardCountPairs.Select(x => new Model.CardCountPair
                    {
                        count = x.count,
                        cardInfoReference = new AssetReference(x.assetGuid),
                    }).ToArray(),
                };
            }
        }
    }
}