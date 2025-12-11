using System.Collections;
using DG.Tweening;
using Henmova.Hit;
using Henmova.Vaibhav;
using UnityEngine;

namespace Henmova
{
    public class Door : MonoBehaviour, IHover, IInteractaction
    {
        public bool Pause_Player { get; set; }
        public bool Can_Manually_Escape { get; set; }
        public Transform hoverPoint;

        public Transform anchore;
        public Vector3 DoorOpen;
        public Vector3 DoorClose;
        public float doorOPenTime;

        bool isDoorOpen;

        public bool IsKeyDoor;
        public string NAME;

        [SerializeField] private AudioSource _sound_Open, _sound_Lock;

        private void Start()
        {
            DoorClose = anchore.localEulerAngles;
        }

        public void Hover(bool isHovering)
        {
            // string m = "Door";
            UI_m_Manager.OnHoverInfo?.Invoke(Game_Manager.instance.PlayerHaskey ? "Escape!" : "Locked! Need A Key!", isHovering, hoverPoint);
        }

        public void Interact(Transform inspector, bool isInteracting)
        {
            if (Game_Manager.instance.PlayerHaskey)
            {
                _sound_Open.Play();
            }
            else
            {
                _sound_Lock.Play();
            }
            if (!Game_Manager.instance.PlayerHaskey) return;
            OpenDoor(!isDoorOpen);
        }

        public void Interact(bool isInteracting)
        {
            throw new System.NotImplementedException();
        }

        void OpenDoor(bool open)
        {
            if (isDoorOpen == open) return;

            if (open && IsKeyDoor)
            {
                if (!PlayerManager.instance.key.activeSelf)
                    return;
            }

            isDoorOpen = open;


            if (isDoorOpen)
            {

                anchore.DOLocalRotate(DoorOpen, doorOPenTime).SetEase(Ease.InOutQuart);
            }
            else
            {
                anchore.DOLocalRotate(DoorClose, doorOPenTime).SetEase(Ease.InOutQuart);
            }

            StartCoroutine(DelayPlay());


        }

        IEnumerator DelayPlay()
        {
            yield return new WaitForSeconds(1f);
            Game_Manager.instance.onGameWin();
        }
    }
}
