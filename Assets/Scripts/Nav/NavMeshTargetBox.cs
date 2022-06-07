using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Nav
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class NavMeshTargetBox : MonoBehaviour
    {
        public BoxCollider Collider { get; private set; }
        private Transform _ts;

        private void OnEnable()
        {
            gameObject.layer = TL.Layer.Block.LayerInt();
            Collider = GetComponent<BoxCollider>();
            _ts = Collider.transform;
        }

        public NavMeshBuildSource BuildSource()
        {
            return new NavMeshBuildSource
            {
                shape = NavMeshBuildSourceShape.Box,
                transform = Matrix4x4.TRS(_ts.position - (Collider.center / 2), _ts.rotation, _ts.localScale),
                size = Collider.size.ChangeY(0f),
                area = 0,
            };
        }
    }
}
