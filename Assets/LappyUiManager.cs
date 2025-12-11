using TMPro;
using UnityEngine;

namespace Henmova.Hit
{
    public class LappyUiManager : MonoBehaviour
    {
        public TextMeshProUGUI hoverText;
        public Transform hoverTransfrom;
        public Vector3 offset;

        Transform lasthover;
        void Start()
        {
            UI_m_Manager.OnHoverInfo += OnHOver;

        }
        private void OnDestroy()
        {
            UI_m_Manager.OnHoverInfo -= OnHOver;
        }

        private void Update()
        {
          /*  if (lasthover != null)
            {
                Vector3 scrrrenpos = Camera.main.WorldToScreenPoint(lasthover.position);
                hoverTransfrom.position = scrrrenpos + offset;
            }*/
        }

        public void OnHOver(string mess, bool hover, Transform pos)
        {
            hoverTransfrom.gameObject.SetActive(hover);
            hoverText.text = mess;

            lasthover = pos;
        }
    }
}
