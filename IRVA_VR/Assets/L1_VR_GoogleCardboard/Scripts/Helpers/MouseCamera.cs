using UnityEngine;

namespace L1_VR_GoogleCardboard.Scripts.Helpers
{
    /// <summary>
    /// Used only when playing in editor (non-VR) to test gaze interactions.
    /// </summary>
    public class MouseCamera : MonoBehaviour
    {
        [SerializeField][Range(0f, 10f)] private float sensitivityX = 1.5f;
        [SerializeField][Range(0f, 10f)] private float sensitivityY = 2.0f;

        private Camera _vrCamera;

        private void Awake() => _vrCamera = GetComponent<Camera>();

        private void Update()
        {
            // 1 = RMB
            if (!Input.GetMouseButton(1)) return;

            var rotX = Input.GetAxis("Mouse Y") * sensitivityX * -1;
            var rotY = Input.GetAxis("Mouse X") * sensitivityY;

            _vrCamera.transform.eulerAngles += new Vector3(rotX, rotY, 0f);
        }
    }
}
