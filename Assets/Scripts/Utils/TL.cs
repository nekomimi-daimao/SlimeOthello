using System;
using UnityEngine;

namespace Utils
{
    public static class TL
    {
        public enum Tag
        {
            Default = 0,
            NavMeshTarget,
        }

        public static string Name(this Tag tag)
        {
            return tag.ToString();
        }

        [Flags]
        public enum Layer
        {
            Default = 0,
            NavMeshTarget = 1 << 0,
        }

        public static int Mask(this Layer layer)
        {
            return LayerMask.GetMask(layer.ToString());
        }

        public static int LayerInt(this Layer layer)
        {
            return LayerMask.NameToLayer(layer.ToString());
        }
    }
}
