using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace NavMesh
{
    public sealed class NavMeshTarget : MonoBehaviour
    {
        private MeshFilter _meshFilter;
        private Transform _ts;

        private void OnEnable()
        {
            gameObject.tag = TagLayer.Tag.NavMeshTarget.Name();
            _meshFilter = GetComponent<MeshFilter>();
            _ts = _meshFilter.transform;
        }

        public NavMeshBuildSource BuildSource()
        {
            return new NavMeshBuildSource
            {
                shape = NavMeshBuildSourceShape.Mesh,
                sourceObject = _meshFilter.mesh,
                transform = _ts.localToWorldMatrix,
                area = 0
            };
        }
    }
}
