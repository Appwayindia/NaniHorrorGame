using System;
using UnityEngine;

namespace Henmova
{
    public class UI_m_Manager : MonoBehaviour
    {
        [Header("----- ACTIONS -----")]
        public static Action<string, bool,Transform> OnHoverInfo = delegate { };
        public static Action<string, Transform> ONHoverDialogue_Display_Timered = delegate { };
    }
}
