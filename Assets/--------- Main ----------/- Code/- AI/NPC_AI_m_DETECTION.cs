using Henmova.Hit;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Henmova.Vaibhav
{
    public class NPC_AI_m_DETECTION : MonoBehaviour
    {
        [Title("Settings")]
        [SerializeField] private float _detectionAngle = 90f;
        [SerializeField] private float _maxDetectionDistance = 10f;
        [SerializeField] private float _rayUpOffset = 1.5f;
        [SerializeField] private LayerMask _obstacleMask;
        [SerializeField] private SphereCollider _playerDetectionCollider;
        [SerializeField] private bool _gizmos = false;
        private bool _playerFound = false;
        public Transform _target;
        private NPC_AI_m_References _referrences;




        void Start()
        {
            _referrences = GetComponentInParent<NPC_AI_m_References>();
            // _target = Instance_Manager.Player_Instance.transform;
        }

        void Update()
        {

            // if (_playerFound) return;

            float dist = Vector3.Distance(_target.position, transform.position);
            if (dist > _playerDetectionCollider.radius + 1)
            {
                _referrences._isPlayerONTheScreen = false;
                
                if (_playerFound && !AngleDetection())
                {
                    _referrences._main.Set_PlayerIdle();
                }
            }
            else
            {
                if (AngleDetection())
                {
                    _referrences._main.Set_PlayerFound();
                    _playerFound = true;
                    _referrences._isPlayerONTheScreen = true;
                    Debug.Log("✅ Player Caught In FOV");
                }
                else
                {
                    _referrences._isPlayerONTheScreen = false;
                }
            }
        }

        private bool AngleDetection()
        {
            if (_target == null) return false;

            Vector3 directionToTarget = (_target.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            // Check if within field of view
            if (angleToTarget <= _detectionAngle * 0.5f)
            {
                float distanceToTarget = Vector3.Distance(transform.position, _target.position);

                // Raycast to check if wall is blocking
                if (Physics.Raycast(transform.position + Vector3.up * _rayUpOffset, directionToTarget, out RaycastHit hit, distanceToTarget, _obstacleMask, QueryTriggerInteraction.Ignore))
                {
                    if (hit.transform == _target)
                    {
                        // No wall between NPC and player
                        return true;
                    }
                    else
                    {
                        // Something else is blocking
                        Debug.DrawLine(transform.position + Vector3.up * _rayUpOffset, hit.point, Color.red);
                        return false;
                    }
                }
            }

            return false;
        }

        public void Set_PlayerFound(bool value)
        {
            _playerFound = value;
        }

        void OnDrawGizmos()
        {
            if (!_gizmos || _playerDetectionCollider == null) return;

            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, _playerDetectionCollider.radius);

            Vector3 leftBoundary = Quaternion.Euler(0, -_detectionAngle * 0.5f, 0) * transform.forward;
            Vector3 rightBoundary = Quaternion.Euler(0, _detectionAngle * 0.5f, 0) * transform.forward;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + leftBoundary * _playerDetectionCollider.radius);
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary * _playerDetectionCollider.radius);
        }
    }
}
