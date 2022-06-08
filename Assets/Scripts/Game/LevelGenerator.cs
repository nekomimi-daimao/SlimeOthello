using System.Linq;
using Block;
using Cysharp.Threading.Tasks;
using Slime;
using UnityEngine;

namespace Game
{
    public sealed class LevelGenerator : MonoBehaviour
    {
        [SerializeField]
        public BlockGenerator blockGenerator;

        [SerializeField]
        public SlimeGenerator slimeGenerator;

        public async UniTask Generate(Level level)
        {
            await blockGenerator.Generate(level.blockMap);

            var positions = blockGenerator.Generated
                .Select(block =>
                {
                    var center = block.navMeshTarget.Collider.center;
                    var half = block.navMeshTarget.Collider.size.y / 2;
                    return center + Vector3.up * (1f + half);
                })
                .ToArray();
            await slimeGenerator.GeneratePopulation(level.slimePopulation, positions);
        }

#if UNITY_EDITOR
        [SerializeField]
        private Level levelInspector;

        [ContextMenu(nameof(GenerateLevelInspector))]
        private void GenerateLevelInspector()
        {
            Generate(levelInspector).Forget();
        }
#endif
    }
}
