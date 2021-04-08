using System;
using UnityEngine.AddressableAssets;

namespace Tofunaut.GridCCG.Game
{
    public class GamePlayer
    {
        public enum Controller
        {
            Local,
            Network,
            AI,
        }

        public readonly int index;
        
        private readonly Model _model;

        public GamePlayer(Model model, int index)
        {
            this.index = index;
            _model = model;
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
        }
    }
}