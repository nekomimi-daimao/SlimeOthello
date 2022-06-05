using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Nav
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class NavMeshTargetBox : MonoBehaviour
    {
        private BoxCollider _collider;
        private Transform _ts;

        private void OnEnable()
        {
            gameObject.layer = TL.Layer.Block.LayerInt();
            _collider = GetComponent<BoxCollider>();
            _ts = _collider.transform;
        }

        public NavMeshBuildSource BuildSource()
        {
            return new NavMeshBuildSource
            {
                shape = NavMeshBuildSourceShape.Box,
                transform = Matrix4x4.TRS(_ts.position - (_collider.center / 2), _ts.rotation, _ts.localScale),
                size = _collider.size.ChangeY(0f),
                area = 0,
            };
        }
    }
}
