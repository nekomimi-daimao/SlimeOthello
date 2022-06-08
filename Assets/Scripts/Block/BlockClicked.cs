using System.Collections.Generic;
using System.Linq;
using Slime;
using UniRx;
using UnityEngine;

namespace Block
{
    public sealed class BlockClicked
    {
        private readonly Block[,] _blockMap;
        private readonly CompositeDisposable _compositeDisposable = new();

        public BlockClicked(Block[,] blockMap, IReadOnlyCollection<Block> generated)
        {
            _blockMap = blockMap;
            generated.Select(block => block.OnClicked)
                .Merge()
                .Subscribe(Clicked)
                .AddTo(_compositeDisposable);
        }

        private void Clicked(Block block)
        {
            var c = block.column;
            var r = block.row;

            var map = ToMap();

            var vec = new Vector2Int[]
            {
                new(-1, -1), new(-1, 0), new(-1, 1),
                new(0, -1), new(0, 1),
                new(1, -1), new(1, 0), new(1, 1),
            };

            foreach (var vector2Int in vec)
            {
            }
        }

        public SlimeBelongs[,] ToMap()
        {
            var col = _blockMap.GetLength(0);
            var row = _blockMap.GetLength(1);
            var map = new SlimeBelongs[col, row];

            for (var c = 0; c < col; c++)
            {
                for (var r = 0; r < row; r++)
                {
                    var flag = _blockMap[c, r].CheckOn()
                        .Select(slime => slime.belongs.Value)
                        .Aggregate((a, b) => a | b);
                    map[c, r] = flag;
                }
            }

            return map;
        }
    }
}
