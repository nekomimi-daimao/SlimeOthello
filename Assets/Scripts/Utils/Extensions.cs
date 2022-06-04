using UnityEngine;

namespace Utils
{
    public static class Extensions
    {
        public static Vector3 ChangeX(this Vector3 vector3, float x)
        {
            vector3.x = x;
            return vector3;
        }

        public static Vector3 ChangeY(this Vector3 vector3, float y)
        {
            vector3.y = y;
            return vector3;
        }

        public static Vector3 ChangeZ(this Vector3 vector3, float z)
        {
            vector3.z = z;
            return vector3;
        }
    }
}
