using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Nav
{
    public sealed class NavMeshBaker : MonoBehaviour
    {
        [SerializeField]
        public int AgentId = 1;

        [SerializeField]
        public bool AutoBake = true;

        private NavMeshDataInstance _instance;

        private void OnEnable()
        {
            if (AutoBake)
            {
                Bake();
            }
        }

        private void OnDisable()
        {
            if (_instance.valid)
            {
                NavMesh.RemoveNavMeshData(_instance);
            }
        }

        [ContextMenu(nameof(Bake))]
        public void Bake()
        {
            var components = new List<MeshFilter>();
            var thisRenderer = GetComponent<MeshFilter>();
            if (thisRenderer != null)
            {
                components.Add(thisRenderer);
            }

            // components.AddRange(GetComponentsInChildren<MeshFilter>());

            var source = components.Select(mr =>
            {
                var source = new NavMeshBuildSource();
                source.shape = NavMeshBuildSourceShape.Mesh;
                source.sourceObject = mr.mesh;
                source.transform = mr.transform.localToWorldMatrix;
                source.area = 0;
                return source;
            }).ToList();

            Debug.Log(source.Count);

            var ts = this.transform;

            var position = ts.position;
            var data = NavMeshBuilder.BuildNavMeshData(
                NavMesh.GetSettingsByIndex(AgentId),
                source,
                new Bounds(position, Vector3.one * 100f),
                position, ts.rotation
            );
            _instance = NavMesh.AddNavMeshData(data);
        }
    }
}
