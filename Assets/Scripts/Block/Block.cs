using System;
using System.Linq;
using Nav;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

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

        [SerializeField]
        public NavMeshTargetBox navMeshTarget;

        public readonly Subject<Block> OnClicked = new();

        private void Awake()
        {
            var trigger = this.gameObject.GetComponent<EventTrigger>();
            var entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener(_ => OnClicked.OnNext(this));
            trigger.triggers.Add(entry);
        }

        private Collider[] _colliders = new Collider[4];

        public Slime.Slime[] CheckOn()
        {
            var colliderSize = navMeshTarget.Collider.size;
            var hitCount = Physics.OverlapBoxNonAlloc(
                navMeshTarget.Collider.center + (Vector3.up * colliderSize.y),
                colliderSize,
                _colliders,
                Quaternion.identity,
                TL.Layer.Slime.Mask()
            );
            if (hitCount == 0)
            {
                return Array.Empty<Slime.Slime>();
            }

            return _colliders
                .Where(c => c != null)
                .Select(c => c.GetComponent<Slime.Slime>())
                .Where(s => s != null)
                .ToArray();
        }
    }
}
