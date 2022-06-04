using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Nav
{
    public class RuntimeNavMeshBaker : MonoBehaviour
    {
        private NavMeshDataInstance _instance;
        private NavMeshData _data;

        private void OnEnable()
        {
            BakeData();
        }

        private void OnDisable()
        {
            if (_instance.valid)
            {
                NavMesh.RemoveNavMeshData(_instance);
            }
        }


        [ContextMenu(nameof(BakeData))]
        public async void BakeData()
        {
            if (_data == null || !_instance.valid)
            {
                _data = new NavMeshData();
                _instance = NavMesh.AddNavMeshData(_data);
            }

            var source = GetComponentsInChildren<NavMeshTargetBox>()
                .Select(box => box.BuildSource())
                .ToList();

            if (source.Count == 0)
            {
                return;
            }

            await NavMeshBuilder.UpdateNavMeshDataAsync(
                _data,
                NavMesh.GetSettingsByIndex(1),
                source,
                new Bounds(Vector3.zero, Vector3.one * 10f)
            );
        }
    }
}
