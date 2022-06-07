using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Slime
{
    public sealed class SlimeGenerator : MonoBehaviour
    {
        [SerializeField]
        private Slime slimePrefab;

        public readonly ReactiveCollection<Slime> Generated = new();

        public async UniTask Generate(SlimePopulation population, Vector3[] positions)
        {
            var belongsAll = (SlimeBelongs[])Enum.GetValues(typeof(SlimeBelongs));
            var belongs = belongsAll.Select(b =>
                {
                    if (b == SlimeBelongs.Leaf)
                    {
                        return Array.Empty<SlimeBelongs>();
                    }

                    var array = new SlimeBelongs[population.Population(b)];
                    Array.Fill(array, b);
                    return array;
                })
                .Aggregate((a, b) => a.Concat(b).ToArray())
                .OrderBy(_ => Guid.NewGuid()).ToArray();

            Clear();

            await UniTask.SwitchToMainThread();

            var ts = this.transform;
            var random = new System.Random();
            foreach (var b in belongs)
            {
                var target = positions[random.Next(positions.Length)];
                var slime = Instantiate(
                    slimePrefab,
                    target, Quaternion.identity, ts
                );
                slime.belongs.Value = b;
                Generated.Add(slime);
            }
        }

        [ContextMenu(nameof(Clear))]
        private void Clear()
        {
            foreach (var g in Generated)
            {
                Destroy(g.gameObject);
            }

            Generated.Clear();
        }
    }
}
