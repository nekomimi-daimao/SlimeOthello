using System;
using UnityEngine;

namespace Slime
{
    [CreateAssetMenu(fileName = "SlimePopulation", menuName = "SlimeOthello/SlimePopulation", order = 2)]
    [Serializable]
    public sealed class SlimePopulation : ScriptableObject
    {
        public int neutral = 1;
        public int leaf = 2;
        public int army = 2;
        public int king = 0;

        public int Population(SlimeBelongs belongs)
        {
            return belongs switch
            {
                SlimeBelongs.Neutral => neutral,
                SlimeBelongs.Leaf => leaf,
                SlimeBelongs.Army => army,
                SlimeBelongs.King => king,
                _ => throw new ArgumentOutOfRangeException(nameof(belongs), belongs, null)
            };
        }
    }
}
