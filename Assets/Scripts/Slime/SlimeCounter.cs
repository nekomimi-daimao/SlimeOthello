using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace Slime
{
    public sealed class SlimeCounter : IDisposable
    {
        private readonly ReactiveDictionary<SlimeBelongs, int> _count = new();
        private readonly CompositeDisposable _compositeDisposable = new();
        public IReadOnlyReactiveDictionary<SlimeBelongs, int> Count => _count;

        private struct BelongsFromTo
        {
            internal readonly SlimeBelongs From;
            internal readonly SlimeBelongs To;

            public BelongsFromTo(SlimeBelongs from, SlimeBelongs to)
            {
                this.From = from;
                this.To = to;
            }
        }

        public SlimeCounter(IReadOnlyCollection<Slime> generated)
        {
            var belongsArray = (SlimeBelongs[])Enum.GetValues(typeof(SlimeBelongs));
            foreach (var slimeBelongs in belongsArray)
            {
                _count[slimeBelongs] = generated.Count(slime => slime.belongs.Value == slimeBelongs);
            }

            generated.Select(
                    slime => slime.belongs
                        .Zip(slime.belongs.Skip(1), (x, y) => new BelongsFromTo(x, y)))
                .Merge()
                .Subscribe(UpdateCount)
                .AddTo(_compositeDisposable);
        }

        private void UpdateCount(BelongsFromTo fromTo)
        {
            _count[fromTo.From] -= 1;
            _count[fromTo.To] += 1;
        }

        public void Dispose()
        {
            _count?.Dispose();
            _compositeDisposable?.Dispose();
        }
    }
}
