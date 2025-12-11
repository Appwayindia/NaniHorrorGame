using UnityEngine;

namespace Henmova
{
    public class QuickFollow : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _offset;
        void FixedUpdate()
        {
            transform.position = _target.position + _offset;
            transform.rotation = _target.rotation;
        }
    }
}
