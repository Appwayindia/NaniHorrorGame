using UnityEngine;

namespace Henmova
{
    public interface IInteractaction
    {
        public bool Pause_Player { get; set; }
        public bool Can_Manually_Escape { get; set; }
        public void Interact(Transform inspector, bool isInteracting);
        public void Interact(bool isInteracting);
    }
}
