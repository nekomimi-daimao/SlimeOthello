using UniRx;
using UnityEngine;
using Utils;

namespace Slime
{
    public sealed class Slime : MonoBehaviour
    {
        public ReactiveProperty<SlimeBelongs> belongs = new(SlimeBelongs.Neutral);
        public ReactiveProperty<SlimeAct> act = new(SlimeAct.Idle);

        [SerializeField]
        private SlimeSwitch slimeSwitch;

        [SerializeField]
        public SlimeMove slimeMove;

        private void Awake()
        {
            this.gameObject.layer = TL.Layer.Slime.LayerInt();
            slimeSwitch.Init(this);
            slimeMove.Init(this);
        }
    }
}
