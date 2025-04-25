using UnityEngine;

namespace L1_VR_GoogleCardboard.Scripts.BalloonGame
{
    /// <summary>
    /// Trigger which destroys balloons which touch it.
    /// </summary>
    public class BalloonPopper : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var potentialBalloon = other.GetComponent<BalloonController>();
            potentialBalloon?.DestroyBalloon(hasBeenPoppedByPlayer: false); // Null check with '?'
        }
    }
}
