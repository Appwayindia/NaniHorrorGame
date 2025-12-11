using DG.Tweening;
using Henmova.Vaibhav;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Henmova.Hit
{
    public class NPC_AI_m_References : MonoBehaviour
    {
        public Animator _animator;
        public NavMeshAgent _navMeshAgent;
        public Collider _collider;
        public Transform _body;
        public NPC_AI_m_DETECTION _detections;
        public NPC_AI_M_Main _main;
        [GUIColor("yellow")]
        public Transform _StartPoint, _RestartPoint;

        [Title("Objects")]
        public GameObject _cylinder, _playerHands;
        public ParticleSystem _blastParticle, _fireParticle;

        // Death State
        internal bool _isDead = false;
        public bool _isGameOver = false;
        public Action _action_OnDead;

        // Animator Keys
        internal int ANIM_KEY_MOVE;


        [Title("---------------- Runtime Data ----------------")]
        public Transform _Player;
        public Vector2 _idleTime_minMax = new Vector2(3f, 8f); public float Get_RandomIdleTime => Random.Range(_idleTime_minMax.x, _idleTime_minMax.y);
        [SerializeField] private Transform _wayPointsHOLDER;
        public float _MoveSpeed = 6f;
        public float _ChaseSpeed = 8f;
        [Header("Limits")]
        public float _DistanceTo_StopChase = 20f;
        public float _DistanceTo_StartAttack = 3f;

        [Title("---------------- Audio ----------------")]
        public AudioSource _sound_Chase;
        public AudioSource _sound_Attack, _sound_TentionMusic, _soundRun;
        public AudioSource _sound_FoundScare, _sound_CatchScare;

        private float _tentionMusicVolOG;

        public RandomAudioPlayer _randomAudioPlayer;
        public CameraShake _explosionShake;
        public bool _shouldPlayRandomNote = true;
        public bool _isPlayerONTheScreen = false;


        private void Awake()
        {
            ANIM_KEY_MOVE = Animator.StringToHash("move");
        }

        void Start()
        {
            //  _Player = Instance_Manager.Player_Instance.transform;
            _main = GetComponent<NPC_AI_M_Main>();

            _tentionMusicVolOG = _sound_TentionMusic.volume;
        }


        // Animations
        public void Animator_SetWalk()
        {
            DOVirtual.Float(
                _animator.GetFloat(ANIM_KEY_MOVE),
                0.5f,
                0.25f,
                newValue => _animator.SetFloat(ANIM_KEY_MOVE, newValue)
            );
        }

        public void Animator_SetRun()
        {
            DOVirtual.Float(
                _animator.GetFloat(ANIM_KEY_MOVE),
                1f,
                0.25f,
                newValue => _animator.SetFloat(ANIM_KEY_MOVE, newValue)
            );
        }

        public void Animator_SetIdle()
        {
            DOVirtual.Float(
                _animator.GetFloat(ANIM_KEY_MOVE),
                0f,
                0.25f,
                newValue => _animator.SetFloat(ANIM_KEY_MOVE, newValue)
            );
        }

        public void Animator_SetGive()
        {
            _animator.SetTrigger(KeyValueNames.ANIM_KEY_GIVE);
        }

        public void Animator_SetAttack()
        {
            _animator.SetTrigger(KeyValueNames.ANIM_KEY_Attack);
        }

        #region ----------- Audio --------------
        public void Play_SoundChase()
        {
            _sound_Chase.Play();
            _sound_TentionMusic.volume = _tentionMusicVolOG;
            _sound_TentionMusic.Play();
        }

        public void Stop_SoundChase()
        {
            _sound_TentionMusic.DOFade(0f, 3f);
        }

        public void Play_RunFoot(bool value)
        {
            if (value)
                _soundRun.Play();
            else
                _soundRun.Pause();
        }

        public void Play_SoundFound() => _sound_FoundScare.Play();
        public void Play_SoundCatch() => _sound_CatchScare.Play();

        public void Play_SoundAttack()
        {
            _sound_Attack.Play();
        }

        public void PlayRandomAudio()
        {
            _randomAudioPlayer.PlayRandomClip();
        }
        #endregion

        // ATTACK 
        public void Perform_Attack()
        {
            // _blastParticle.gameObject.SetActive(true);
            Animator_SetAttack();
            StartCoroutine(DelayAttack());
        }

        IEnumerator DelayAttack()
        {
            _Player.DOLookAt(new Vector3(transform.position.x, _Player.position.y, transform.position.z), 0.6f).SetEase(Ease.OutExpo);
            if (!_sound_Chase.isPlaying)
                Play_SoundAttack();
            yield return new WaitForSeconds(.5f);
            _playerHands.SetActive(false);
            _cylinder.gameObject.SetActive(false);
            _explosionShake.Shake(3f, 1f);
            _blastParticle.gameObject.SetActive(true);
            _fireParticle.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            Game_Manager.instance.OnGameOver();

            // yield return new WaitForSeconds(2f);

        }

        #region ---------------- Getters ----------------
        public Transform Get_RandomUniqueWayPoint(Transform previousPoint)
        {
            Transform newPoint = null;
            int tIndex = Random.Range(0, _wayPointsHOLDER.childCount);
            newPoint = _wayPointsHOLDER.GetChild(tIndex);
            if (previousPoint)
            {
                if (newPoint == previousPoint)
                {
                    while (newPoint == previousPoint)
                    {
                        tIndex = Random.Range(0, _wayPointsHOLDER.childCount);
                        newPoint = _wayPointsHOLDER.GetChild(tIndex);
                    }
                }
            }

            return newPoint;
        }
        #endregion
    }
}
