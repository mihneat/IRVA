using System;
using UnityEngine;

namespace L2_VR_SteamVR_Basics.Scripts
{
    public class BaseballBatController : MonoBehaviour
    {
        [SerializeField] private AudioClip batHit;
        
        private void OnCollisionEnter(Collision other)
        {
            if (!other.transform.CompareTag("Ball"))
                return;
            
            AudioSource.PlayClipAtPoint(batHit, other.contacts[0].point, 1.0f);
        }
    }
}
