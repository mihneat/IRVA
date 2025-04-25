using System;
using UnityEngine;

namespace L2_VR_SteamVR_Basics.Scripts
{
    public class ProjectileCannonController : MonoBehaviour
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform projectileSpawnTransform;
        [SerializeField] [Range(0.1f, 10)] private float projectileLaunchForce = 1.1f;

        private void Awake()
        {
            // Safety checks.
            if(projectilePrefab == null || projectileSpawnTransform == null)
            {
                Debug.LogError("[ProjectileCannonController] Some inspector values have not been assigned!");
                throw new NullReferenceException();
            }
        }

        public void LaunchProjectile()
        {
            // Instantiate projectile, get launch direction & apply a force of type `Impulse` upon the `Rigidbody` component.
            var projInst = Instantiate(projectilePrefab, projectileSpawnTransform.position, Quaternion.identity);
            var projRb = projInst.GetComponent<Rigidbody>();
            var projDir = projectileSpawnTransform.transform.up;
            projRb.AddForce(projDir * projectileLaunchForce, ForceMode.Impulse);
        }
    }
}