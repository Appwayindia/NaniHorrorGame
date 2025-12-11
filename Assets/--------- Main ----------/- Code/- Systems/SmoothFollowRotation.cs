using UnityEngine;

namespace GamesOfVaibhav
{
    public class SmoothFollowRotation : MonoBehaviour
    {
        [Header("Target Settings")]
        public Transform target;

        [Header("Rotation Settings")]
        [Tooltip("How fast the object rotates to match the target’s rotation")]
        public float rotationSmoothSpeed = 5f;

        void LateUpdate()
        {
            if (!target) return;

            // Follow position directly (no smoothing)
            transform.position = target.position;

            // Smoothly follow rotation
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                target.rotation,
                Time.deltaTime * rotationSmoothSpeed
            );
        }
    }
}
