using System;
using System.Collections;
using DG.Tweening;
using Henmova.Hit;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Henmova.Vaibhav
{
    public class Game_Manager : MonoBehaviour
    {

        public static Game_Manager instance;
        [Title("--------------------- Game Stats --------------------")]
        [SerializeField] private int _MaxPlayerLives = 3;
        [ReadOnly] public int _currentLives;

        [Title("--------------------- Actions --------------------")]
        public static Action OnPlayerCaughtBy_Enemy = delegate { };
        public static Action OnPlayerRestart = delegate { };
        public static Action OnPlayerDead = delegate { };


        public GameObject HomeScreen;

        public Button playbtn;

        public bool MobileInput;
        public GameObject GameplayPanel;
        public CanvasGroup FailPanel, WinPanel;
        public GameObject MobilePanel;
        public GameObject PcPanel;
        public Button InteractBtn;
        public Joystick joystick;
        public MobileLookInput lookInput;
        public FixedTouchField fixedTouchField;
        public Button vabInsta, hitInsta;
        public CanvasGroup group;
        public GameObject gamewinPanel;

        public Transform[] keyPoints;
        public Transform key;
        public Button restartButton;
        #region --------------- MONO ----------------------


        public Vector2 move, look;
        public Action Action_OnInteract;
        public static Action OnGameWon = delegate { };
        public bool PlayerHaskey = false;
        public bool GameWon = false;

        bool pause;


        private void Awake()
        {
            instance = this;

        }
        void Start()
        {
            pause = true;
            OnPlayerCaughtBy_Enemy += Handle_PlayerCaughtBy_Enemy;
            restartButton.onClick.AddListener(onreloadScene);

            _currentLives = _MaxPlayerLives;
            Time.timeScale = 0;

            playbtn.onClick.AddListener(onclickPlay);
            vabInsta.onClick.AddListener(onclickVab);
            hitInsta.onClick.AddListener(onClickHit);

            MobilePanel.SetActive(MobileInput);
            PcPanel.SetActive(!MobileInput);
            if (MobileInput)
            {
                InteractBtn.onClick.AddListener(onInteract);
            }
            if (!MobileInput)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            Transform t = keyPoints[UnityEngine.Random.Range(0, keyPoints.Length)];
            key.GetComponent<Rigidbody>().isKinematic = true;
            key.position = t.position;
            key.rotation = t.rotation;
            for (int i = 0; i < keyPoints.Length; i++)
            {
                keyPoints[i].gameObject.SetActive(false);
            }

            group.alpha = 0.1f;
            group.DOFade(1, 1).SetUpdate(true);
            //   key.GetComponent<Rigidbody>().isKinematic = false;
        }

        private void Update()
        {
            if (pause) return;


            if (MobileInput)
            {
                move = joystick.Direction;
                look = fixedTouchField.TouchDist;
            }
            else
            {
                move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                look = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                if (Input.GetKeyDown(KeyCode.E)) onInteract();
            }


        }
        void onInteract()
        {
            if (pause) return;
            Action_OnInteract?.Invoke();
        }



        void onclickPlay()
        {
            Time.timeScale = 1;
            GameplayPanel.SetActive(true);
            HomeScreen.SetActive(false);
            pause = false;



            if (!MobileInput)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        void onclickVab()
        {
            Application.OpenURL("https://www.instagram.com/gamesofvaibhav");
        }
        void onClickHit()
        {
            Application.OpenURL("https://www.instagram.com/indiangamedevv");
        }



        public void OnGameOver()
        {
            pause = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameplayPanel.SetActive(false);
            FailPanel.gameObject.SetActive(true);
            FailPanel.alpha = 0;
            FailPanel.DOFade(1, 3f).From(0);
            Invoke(nameof(onreloadScene), 4);
        }
        public void onGameWin()
        {
            GameWon = true;

            OnGameWon?.Invoke();
            GameplayPanel.SetActive(false);
            gamewinPanel.SetActive(true);
            WinPanel.DOFade(1, 3f).From(0);

            pause = true;
            if (!MobileInput)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            // Invoke(nameof(onreloadScene), 15f);
        }
        void onreloadScene()
        {
            SceneManager.LoadScene(0);
        }




        void OnDestroy()
        {
            OnPlayerCaughtBy_Enemy -= Handle_PlayerCaughtBy_Enemy;
        }

        #endregion

        #region ---------------- Listeners ------------------------
        private void Handle_PlayerCaughtBy_Enemy()
        {
            _currentLives--;

            if (_currentLives <= 0)
            {
                // dead
                OnPlayerDead?.Invoke();
            }
            else
            {
                // restart
                OnPlayerRestart?.Invoke();
            }
        }

        // add delay
        IEnumerator DelayCheckForPlay()
        {
            yield return new WaitForSeconds(1f);
        }
        #endregion
    }
}
