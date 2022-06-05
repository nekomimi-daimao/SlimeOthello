using UniRx;
using UnityEngine;

namespace Slime
{
    public sealed class Slime : MonoBehaviour
    {
        public ReactiveProperty<SlimeBelongs> belongs = new(SlimeBelongs.Neutral);
        public ReactiveProperty<SlimeAct> act = new(SlimeAct.Idle);

        [SerializeField]
        private SlimeSwitch slimeSwitch;

        [SerializeField]
        private SlimeMove slimeMove;

        private void Awake()
        {
            slimeSwitch.Init(this);
            slimeMove.Init(this);
        }
    }
}
