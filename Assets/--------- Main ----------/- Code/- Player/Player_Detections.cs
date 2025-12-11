using Henmova.Hit;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Henmova.Vaibhav
{
    public class Player_Detections : MonoBehaviour
    {
        [Title("Components")]
        private Player_Referrences _referrences;
        [Title("RayCast Properties")]
        [SerializeField] private Transform _ray_Point;
        [SerializeField] private LayerMask _ray_LayerMask;
        [SerializeField] private float _ray_Distance = 5f;
        private GameObject _lastHit;

        [SerializeField] private KeyCode _interact_Key = KeyCode.E;
        [SerializeField] private KeyCode _interact_Cancel_Key = KeyCode.Escape;
        [SerializeField] private Button _interactButton;

        #region -- MONO --

        void Awake()
        {
            _referrences = GetComponent<Player_Referrences>();

            _interactButton.onClick.AddListener(Interact);
        }
        void OnEnable()
        {
        }
        void OnDisable()
        {

        }

        void Update()
        {
            Detect_Ray();
            Proceed_WithDetections();
        }
        #endregion

        #region -- PRIVATE METHODS --

        public void Interact()
        {
            if (_lastHit)
                if (_lastHit.TryGetComponent(out IInteractaction interactable))
                {
                    // Interact with the object
                    interactable.Interact(_ray_Point, true);

                    // informing listeners
                    // if (interactable.Pause_Player)
                    //     _referrences.PlayerManager.On_TalkToAi.Invoke(true, _lastHit.transform);
                }
        }
        private void Proceed_WithDetections()
        {
            // getting scrips
            if (_lastHit == null) return;
            if (Input.GetKeyDown(_interact_Key))
            {

                if (_lastHit.TryGetComponent(out IInteractaction interactable))
                {
                    // Interact with the object
                    interactable.Interact(_ray_Point, true);

                    // informing listeners
                    // if (interactable.Pause_Player)
                    //     _referrences.PlayerManager.On_TalkToAi.Invoke(true, _lastHit.transform);
                }
            }
            if (Input.GetKeyDown(_interact_Cancel_Key))
            {
                if (_lastHit == null) return;
                if (_lastHit.TryGetComponent(out IInteractaction interactable))
                {
                    Debug.Log("THIS");
                    if (interactable.Can_Manually_Escape)
                    {
                        // Cancel interaction with the object
                        interactable.Interact(_ray_Point, false);
                        // informing listeners
                        // if (interactable.Pause_Player)
                        //     _referrences.PlayerManager.On_TalkToAi.Invoke(false, null);
                    }
                }
            }

        }
        private void Detect_Ray()
        {
            if (Physics.Raycast(_ray_Point.position, _ray_Point.forward, out RaycastHit hit, _ray_Distance, _ray_LayerMask, QueryTriggerInteraction.Ignore))
            {
                if (_lastHit != hit.collider.gameObject)
                {
                    // Hiting the ray to the new object
                    // disable old hover if has
                    IHover hoverable;
                    if (_lastHit)
                        if (_lastHit.TryGetComponent(out hoverable))
                        {
                            // Hover over the object
                            hoverable.Hover(false);
                        }

                    // apply new hover
                    _lastHit = hit.collider.gameObject;
                    if (_lastHit.TryGetComponent(out hoverable))
                    {
                        // Hover over the object
                        hoverable.Hover(true);
                    }
                }
            }
            else
            {
                // If the raycast does not hit anything, reset the last hit object
                if (_lastHit != null)
                {
                    if (_lastHit.TryGetComponent(out IHover hoverable))
                    {
                        // Stop hovering over the object
                        hoverable.Hover(false);
                    }
                    _lastHit = null;
                }
            }
        }
        #endregion


        #region ------ LISTENERS ------
        private void Handle_EndDialgoe()
        {
            if (_lastHit)
                if (_lastHit.TryGetComponent(out IInteractaction interactable))
                {
                    // Cancel interaction with the object
                    interactable.Interact(_ray_Point, false);
                    // informing listeners
                }
            // _referrences.PlayerManager.On_TalkToAi.Invoke(false, null);
            _lastHit = null; // Reset last hit object after dialogue ends
        }
        #endregion
    }
}
