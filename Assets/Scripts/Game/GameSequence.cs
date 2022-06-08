using Cysharp.Threading.Tasks;
using Slime;
using UnityEngine;

namespace Game
{
    public sealed class GameSequence : MonoBehaviour
    {
        [SerializeField]
        private LevelGenerator levelGenerator;

        private async UniTask Init()
        {
            await levelGenerator.Generate(new Level());
            var blocks = levelGenerator.blockGenerator.Generated;
            var slimes = levelGenerator.slimeGenerator.Generated;

            var slimeCounter = new SlimeCounter(slimes);
            

        }
        
        
    }
}
