using Nav;
using UnityEngine;

namespace Block
{
    [RequireComponent(typeof(NavMeshTargetBox))]
    public sealed class Block : MonoBehaviour
    {
        public BlockState BlockState;

        public int Column;
        public int Row;
    }
}
