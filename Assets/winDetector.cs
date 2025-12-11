using Henmova.Vaibhav;
using UnityEngine;

namespace Henmova
{
    public class winDetector : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                Game_Manager.instance.onGameWin();
            }
        }
    }
}
