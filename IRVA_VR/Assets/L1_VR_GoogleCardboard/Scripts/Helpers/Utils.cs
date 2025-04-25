using UnityEngine;

namespace L1_VR_GoogleCardboard.Scripts.Helpers
{
    public static class Utils
    {
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static Vector3 GetRandomPointInBounds(Bounds b)
        {
            return new Vector3(
                Random.Range(b.min.x, b.max.x),
                Random.Range(b.min.y, b.max.y),
                Random.Range(b.min.z, b.max.z));
        }
    }
}
