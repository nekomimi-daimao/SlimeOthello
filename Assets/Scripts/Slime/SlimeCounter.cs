using System;
using System.Linq;
using UniRx;

namespace Slime
{
    public sealed class SlimeCounter : IDisposable
    {
        private readonly ReactiveDictionary<SlimeBelongs, int> _count = new();
        private readonly CompositeDisposable _compositeDisposable = new();
        private readonly IReadOnlyReactiveCollection<Slime> _generated;
        private readonly SlimeBelongs[] _belongsArray = (SlimeBelongs[])Enum.GetValues(typeof(SlimeBelongs));
        public IReadOnlyReactiveDictionary<SlimeBelongs, int> Count => _count;

        public SlimeCounter(IReadOnlyReactiveCollection<Slime> generated)
        {
            _generated = generated;

            generated.Select(
                    slime => slime.belongs)
                .Merge()
                .AsUnitObservable()
                .Subscribe(UpdateCount)
                .AddTo(_compositeDisposable);

            generated.ObserveCountChanged()
                .AsUnitObservable()
                .Subscribe(UpdateCount)
                .AddTo(_compositeDisposable);
        }

        private void UpdateCount(Unit _)
        {
            foreach (var slimeBelongs in _belongsArray)
            {
                _count[slimeBelongs] = _generated.Count(slime => slime.belongs.Value == slimeBelongs);
            }
        }

        public void Dispose()
        {
            _count?.Dispose();
            _compositeDisposable?.Dispose();
        }
    }
}
