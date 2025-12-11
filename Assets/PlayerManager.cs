using UnityEngine;

namespace Henmova.Hit
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager instance;

        public GameObject key;


        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            key.SetActive(false);
        }


        public void PickupKey()
        {
            key.SetActive(true);
        }
    }
}
