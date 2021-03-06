using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Utils;

namespace Block
{
    public sealed class BlockGenerator : MonoBehaviour
    {
        [SerializeField]
        private Block empty;

        [SerializeField]
        private Block walkable;

        [SerializeField]
        private Block unWalkable;

        private Dictionary<BlockState, Block> _prefabDic;

        public readonly ReactiveCollection<Block> Generated = new();

        public Block[,] CreatedMap { get; private set; }

        private const float IntervalSec = 0.2f;

        private void Awake()
        {
            _prefabDic = new Dictionary<BlockState, Block>
            {
                [BlockState.Empty] = empty,
                [BlockState.Walkable] = walkable,
                [BlockState.UnWalkable] = unWalkable
            };
        }

        public async UniTask Generate(BlockMap blockMap, float interval = IntervalSec)
        {
            await UniTask.SwitchToMainThread();

            Clear();

            CreatedMap = new Block[blockMap.Column, blockMap.Row];

            var ts = this.transform;
            var center = ts.position;

            var centerC = blockMap.Column / 2;
            var centerR = blockMap.Row / 2;

            for (var c = blockMap.Column - 1; c >= 0; c--)
            {
                for (var r = 0; r < blockMap.Row; r++)
                {
                    var blockState = blockMap.Map[c, r];
                    var prefab = _prefabDic[blockState];
                    var instance = Instantiate(
                        prefab,
                        center + new Vector3(-centerR + r, 0f, -centerC + c),
                        Quaternion.identity,
                        ts
                    );
                    instance.blockState = blockState;
                    instance.column = c;
                    instance.row = r;
                    Generated.Add(instance);
                    CreatedMap[c, r] = instance;
                    await UniTask.Delay(TimeSpan.FromSeconds(interval));
                }
            }

            var cage = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            cage.SetParent(ts);
            cage.localPosition = Vector3.zero;
            cage.localRotation = Quaternion.identity;
            cage.localScale = new Vector3(blockMap.Row, 6f, blockMap.Column);
            cage.gameObject.layer = TL.Layer.Cage.LayerInt();

            Destroy(cage.GetComponent<Renderer>());
            Destroy(cage.GetComponent<Collider>());
            await UniTask.Yield();

            var mesh = cage.GetComponent<MeshFilter>().mesh;
            mesh.triangles = mesh.triangles.Reverse().ToArray();
            cage.gameObject.AddComponent<MeshCollider>();
        }

        [ContextMenu(nameof(Clear))]
        private void Clear()
        {
            foreach (var g in Generated)
            {
                Destroy(g.gameObject);
            }

            Generated.Clear();
            CreatedMap = new Block[0, 0];
        }

#if UNITY_EDITOR
        [Space]
        public BlockMap map;

        [ContextMenu(nameof(GenerateMap))]
        private void GenerateMap()
        {
            Generate(map).Forget();
        }
#endif
    }
}
