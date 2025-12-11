using UnityEngine;

namespace Henmova
{
    public class Player_Instance : MonoBehaviour
    {
        public static Player_Instance Instance;
        void Awake()
        {
            Instance = this;
        }
    }
}
