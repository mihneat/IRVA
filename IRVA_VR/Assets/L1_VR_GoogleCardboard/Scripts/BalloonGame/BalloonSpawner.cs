using System.Collections;
using L1_VR_GoogleCardboard.Scripts.Helpers;
using UnityEngine;

namespace L1_VR_GoogleCardboard.Scripts.BalloonGame
{
    /// <summary>
    /// Used to control the spawn behavior of balloons.
    /// </summary>
    public class BalloonSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject balloonPrefab;
        [SerializeField] [Range(0.1f, 5f)] private float spawnRate = 1.5f;

        private BoxCollider _boxCollider;

        private void Awake() => _boxCollider = GetComponent<BoxCollider>();

        private void Start() => StartCoroutine(SpawnBalloonsRoutine());

        private IEnumerator SpawnBalloonsRoutine()
        {
            for (;;)
            {
                // TODO 3.1 : Determine the spawning position for the balloon.
                //            Balloons should spawn at a random position inside the volume of the bounding box.
                //            Hint: Use the `BoxCollider` components (some attribute from it) & check the `Utils` class.
                var spawnPos = Utils.GetRandomPointInBounds(_boxCollider.bounds);
                
                if(balloonPrefab != null)
                {
                    // TODO 3.2 : Spawn the balloon at the previously determined position.
                    Instantiate(balloonPrefab, spawnPos, Quaternion.identity);
                }
                else
                {
                    Debug.LogWarning("'balloonPrefab' is NULL!");
                }
                
                yield return new WaitForSeconds(spawnRate);
            }
        }
    }
}
