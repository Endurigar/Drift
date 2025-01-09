using Fusion;
using UnityEngine;

namespace Multiplayer
{
    public class CameraFollower : NetworkBehaviour
    {
        [Header("Target Settings")] public Transform target;
        [Header("Offset Settings")] public Vector3 offset = new Vector3(0, 5, -10);
        [Header("Smooth Settings")] public float smoothSpeed = 0.125f;
        public void CameraSet()
        {
            if (target == null)
            {
                Debug.LogWarning("Target is not assigned for the CameraFollow script.");
                return;
            }
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
            transform.LookAt(target);
        }
    }
}