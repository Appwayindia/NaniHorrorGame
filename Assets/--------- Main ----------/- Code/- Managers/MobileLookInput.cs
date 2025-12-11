using UnityEngine;
using UnityEngine.EventSystems;

namespace Henmova.Hit
{
    public class MobileLookInput : MonoBehaviour,  IPointerDownHandler, IPointerUpHandler
    {

        [Header("Settings")]
        public float sensitivity = 0.2f;
        public float SmoothTime = 0.2f;

        [Header("Output (Read Only)")]
        public Vector2 look; // X = yaw, Y = pitch

        private Vector2 lastTouchPos;
        private int lookFingerId = -1;
        bool IsTouched;
        Touch touch;

        Vector3 raw;
      /*  void Update()
        {
            // look = Vector2.zero; // reset each frame
            raw = Vector3.zero;
            foreach (Touch touch in Input.touches)
            {
                // Only accept touches on the right half of the screen
                if (touch.position.x > Screen.width / 2)
                {
                    if (touch.phase == TouchPhase.Began && lookFingerId == -1)
                    {
                        lookFingerId = touch.fingerId;
                        lastTouchPos = touch.position;
                    }
                    else if (touch.fingerId == lookFingerId)
                    {
                        if (touch.phase == TouchPhase.Moved)
                        {
                            Vector2 delta = (touch.position - lastTouchPos) * sensitivity;
                            lastTouchPos = touch.position;

                            // Save look delta
                            // look = new Vector2(delta.x, -delta.y);
                            raw = new Vector2(delta.x, -delta.y);

                        }
                        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                        {
                            lookFingerId = -1;
                        }
                    }
                }
            }

            // look = Vector2.SmoothDamp(look, raw, ref look, SmoothTime* Time.deltaTime);
            look = Vector2.Lerp(look, raw, SmoothTime * Time.deltaTime);
        }*/
        void Update()
        {
            // look = Vector2.zero; // reset each frame
            raw = Vector3.zero;
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    // Only accept touches on the right half of the screen
                    if (IsTouched)
                    {
                        touch = Input.GetTouch(i);
                        /* if (touch.phase == TouchPhase.Began && lookFingerId == -1)
                         {
                             lookFingerId = touch.fingerId;
                             lastTouchPos = touch.position;
                         }
                         else*/
                        if (touch.fingerId == lookFingerId)
                        {
                            if (touch.phase == TouchPhase.Moved)
                            {
                                Vector2 delta = (touch.position - lastTouchPos) * sensitivity;
                                lastTouchPos = touch.position;

                                // Save look delta
                                // look = new Vector2(delta.x, -delta.y);
                                raw = new Vector2(delta.x, -delta.y);

                            }
                            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                            {
                                lookFingerId = -1;
                            }
                        }
                    }
                }
            }

            // look = Vector2.SmoothDamp(look, raw, ref look, SmoothTime* Time.deltaTime);
            look = Vector2.Lerp(look, raw, SmoothTime * Time.deltaTime);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsTouched = true;
            lookFingerId = eventData.pointerId;
            lastTouchPos = eventData.position;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            IsTouched = false;
            lookFingerId = -1;
        }
    }


}