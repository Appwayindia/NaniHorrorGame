using Henmova.Vaibhav;
using UnityEngine;

namespace Henmova.Hit
{
    public class PIckableKey : MonoBehaviour, IHover, IInteractaction
    {

        public bool Pause_Player { get; set; }
        public bool Can_Manually_Escape { get; set; }

        public Transform hoverPoint;
        public Collider collider;
        public Rigidbody rb;
        [SerializeField] private AudioSource _soundKey;
        public void Hover(bool isHovering)
        {
            string m = "Key";
            UI_m_Manager.OnHoverInfo?.Invoke(m, isHovering, hoverPoint);
        }

        public void Interact(Transform inspector, bool isInteracting)
        {
            PickUpKey();
            _soundKey.Play();
            Game_Manager.instance.PlayerHaskey = true;
        }

        public void Interact(bool isInteracting)
        {

        }
         
        void PickUpKey()
        {
            PlayerManager.instance.PickupKey();
            UI_m_Manager.OnHoverInfo?.Invoke("Key", false, hoverPoint);
            gameObject.SetActive(false);
        }
    }
}
