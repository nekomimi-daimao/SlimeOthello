using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Slime
{
    // [RequireComponent(typeof(SlimeMove))]
    public sealed class Slime : MonoBehaviour
    {
        public ReactiveProperty<SlimeBelongs> belongs = new(SlimeBelongs.Neutral);

        [SerializeField]
        private SlimeAnimation neutral;

        [SerializeField]
        private SlimeAnimation leaf;

        [SerializeField]
        private SlimeAnimation army;

        [SerializeField]
        private SlimeAnimation king;

        private SlimeMove _slimeMove;
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

        private void OnEnable()
        {
            belongs.TakeUntilDisable(this).Subscribe(OnChangeBelongs);
        }

        private void OnChangeBelongs(SlimeBelongs belong)
        {
            var target = _slimeDic[belong];
            foreach (var (key, value) in _slimeDic)
            {
                value.gameObject.SetActive(belong == key);
            }

            target.gameObject.OnDisableAsObservable().Subscribe();
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            belongs.SetValueAndForceNotify(belongs.Value);
        }
#endif
    }
}
