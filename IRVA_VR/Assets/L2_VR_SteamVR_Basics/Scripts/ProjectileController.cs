using UnityEngine;

namespace L2_VR_SteamVR_Basics.Scripts
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] private float destroyTime = 60f;

        private void Awake() => Destroy(gameObject, destroyTime);
    }
}
