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
        private BlockGenerator blockGenerator;

        [SerializeField]
        private SlimeGenerator slimeGenerator;

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
            await slimeGenerator.Generate(level.slimePopulation, positions);
        }

#if UNITY_EDITOR
        public Level levelInspector;

        [ContextMenu(nameof(GenerateLevelInspector))]
        public void GenerateLevelInspector()
        {
            Generate(levelInspector).Forget();
        }
#endif
    }
}
