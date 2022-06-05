using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Slime
{
    public sealed class SlimeSwitch : MonoBehaviour
    {
        private Slime _slime;

        [SerializeField]
        private SlimeAnimation neutral;

        [SerializeField]
        private SlimeAnimation leaf;

        [SerializeField]
        private SlimeAnimation army;

        [SerializeField]
        private SlimeAnimation king;

        private Dictionary<SlimeBelongs, SlimeAnimation> _slimeDic;

        private void Awake()
        {
            _slimeDic = new Dictionary<SlimeBelongs, SlimeAnimation>
            {
                { SlimeBelongs.Neutral, neutral },
                { SlimeBelongs.Leaf, leaf },
                { SlimeBelongs.Army, army },
                { SlimeBelongs.King, king },
            };
        }

        internal void Init(Slime slime)
        {
            _slime = slime;
            slime.belongs.TakeUntilDestroy(this).Subscribe(OnChangeBelongs);
        }

        private void OnChangeBelongs(SlimeBelongs belong)
        {
            var target = _slimeDic[belong];
            foreach (var (key, value) in _slimeDic)
            {
                value.gameObject.SetActive(belong == key);
            }

            target.Init(_slime);
        }
    }
}
