using Nav;
using UnityEngine;

namespace Block
{
    [RequireComponent(typeof(NavMeshTargetBox))]
    public sealed class Block : MonoBehaviour
    {
        [SerializeField]
        public BlockState blockState;

        [SerializeField]
        public int column;

        [SerializeField]
        public int row;
    }
}
