using Nav;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Block
{
    [RequireComponent(typeof(NavMeshTargetBox), typeof(EventTrigger))]
    public sealed class Block : MonoBehaviour
    {
        [SerializeField]
        public BlockState blockState;

        [SerializeField]
        public int column;

        [SerializeField]
        public int row;

        public readonly Subject<Block> OnClicked = new();

        private void Awake()
        {
            var trigger = this.gameObject.GetComponent<EventTrigger>();
            var entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener(_ => OnClicked.OnNext(this));
            trigger.triggers.Add(entry);
        }
    }
}
