using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Slime
{
    [RequireComponent(
        typeof(NavMeshAgent),
        typeof(Rigidbody)
    )]
    public sealed class Slime : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private Rigidbody _rigidbody;

        public Vector3 Destination => _agent.destination;

        private void OnEnable()
        {
            _agent = GetComponent<NavMeshAgent>();
            _rigidbody = GetComponent<Rigidbody>();

            _agent.updatePosition = false;
            _agent.updateRotation = true;

            MoveLoop(this.GetCancellationTokenOnDestroy()).Forget();
        }

        public void SetDestination(Vector3 destination)
        {
            _agent.SetDestination(destination);
        }

        private readonly float _nearDistance = Mathf.Pow(1f, 2f);
        private readonly float _speedLimit = Mathf.Pow(3f, 2f);

        private async UniTask MoveLoop(CancellationToken token)
        {
            var ts = this.transform;

            while (!token.IsCancellationRequested)
            {
                await UniTask.WaitForFixedUpdate();

                if (_agent.pathPending)
                {
                    continue;
                }

                // if (_agent.pathStatus == NavMeshPathStatus.PathPartial)
                // {
                //     continue;
                // }
                //

                if (Vector3.SqrMagnitude((_rigidbody.position - _agent.destination).ChangeY(0f)) < _nearDistance)
                {
                    Debug.Log($"near");
                    _rigidbody.velocity *= 0.1f;
                    continue;
                }

                // if (_rigidbody.velocity.sqrMagnitude > _speedLimit)
                // {
                //     Debug.Log("speed");
                //     _rigidbody.velocity = Vector3.up;
                //     continue;
                // }

                Debug.Log("force");

                _rigidbody.AddForce((ts.forward - _rigidbody.velocity) * 4f, ForceMode.Acceleration);
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Slime.Slime))]
public class SlimeEditor : Editor
{
    private Slime.Slime t;
    private Vector3 _destination;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        _destination = EditorGUILayout.Vector3Field("destination", _destination);
        if (GUILayout.Button("Set Destination"))
        {
            t.SetDestination(_destination);
        }
    }

    private void OnEnable()
    {
        t = target as Slime.Slime;
    }
}


#endif
