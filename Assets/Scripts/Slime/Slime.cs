using System;
using UniRx;
using UnityEngine;

namespace Slime
{
    public sealed class Slime : MonoBehaviour
    {
        public ReactiveProperty<SlimeBelongs> belongs = new(SlimeBelongs.Neutral);


        private void OnEnable()
        {
            belongs.TakeUntilDisable(this).Subscribe(OnChangeBelongs);
        }


        private void OnChangeBelongs(SlimeBelongs belong)
        {
            throw new NotImplementedException();
        }
    }
}
