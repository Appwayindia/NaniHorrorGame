using System.Collections;
using Henmova.Hit;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Henmova.Vaibhav
{
    public class NPC_AI_M_Main : Base_Health
    {
        [Title("--- State Control ---")]
        public bool N_AutoInitializeAtStart = false;
        public NPC_State_Type State = NPC_State_Type.None;
        [SerializeField] public NPC_Ai_StateBase CurrentState { get; set; }
        public NPC_Ai_StateFactory StateFactory { get; private set; }
        public NPC_State_Type Last_Paused_State { get; set; } = NPC_State_Type.None;
        [field: SerializeField] public bool Pause_Player { get; set; }
        public float CurrentTarget_RemainingDistance;

        [Title("---- RUNTIME DATA ----")]
        public bool N_HasPath;
        public bool N_IsSummoned { get; set; } = false; // Indicates if the NPC is summoned
        public int N_Summons { get; set; }
        public Transform N_Attack_Target { get; set; }
        public bool Can_Manually_Escape { get; set; } = true;

        public bool _isChasing = false;


        [Title("--- NPC Data ---")]
        public NPC_AI_m_References N_References;
        [Title("--- Runtime Data ---")]


        private void Awake()
        {

            StateFactory = new NPC_Ai_StateFactory(this);

            // temp init
            if (N_AutoInitializeAtStart)
                Initialize();

            // subscription
            Game_Manager.OnPlayerRestart += Handle_PlayerRestart;
            Game_Manager.OnPlayerDead += Handle_PlayerDies;
        }

        private void OnDestroy()
        {
            Game_Manager.OnPlayerRestart -= Handle_PlayerRestart;
            Game_Manager.OnPlayerDead -= Handle_PlayerDies;
        }

        private void Update()
        {
            if (CurrentState != null)
            {
                CurrentState.Update(); // Update the current state
            }
        }

        #region --- PRIVATE METHODS ---
        public virtual void Initialize()
        {
            // signing attributes
            // health
            AddHealth(1000);

            CurrentState = StateFactory.S_Idle;


            CurrentState.SwitchState(); // Enter the initial state
        }

        #endregion

        #region --- PUBLIC METHODS ---
        public void Set_PlayerFound()
        {
            if (_isChasing) return;
            if (N_References._isGameOver) return;
            CurrentState.SwitchState(StateFactory.S_ChaseTarget);
        }

        public void Set_PlayerIdle()
        {
            N_References.Stop_SoundChase();
            CurrentState.SwitchState(StateFactory.S_Idle);
        }
        #endregion


        #region --- PRIVATE METHODS ---
        //temp
        float temp_cspeed, temp_blendvalue;
        public void SetAnimatorMoveSpeed()
        {
            temp_cspeed = N_References._navMeshAgent.velocity.magnitude;
            temp_blendvalue = Mathf.InverseLerp(0f, N_References._ChaseSpeed, temp_cspeed); // gives 0�1
            N_References._animator.SetFloat(N_References.ANIM_KEY_MOVE, temp_blendvalue);
        }
        #endregion

        #region ------ INTER. Health ------

        public override void TakeDamage(int Damage)
        {
            base.TakeDamage(Damage);
        }

        public override void Die()
        {
            base.Die();
            // CurrentState.SwitchState(StateFactory.S_Death);
        }

        #endregion

        #region ------------------------ Listeners -----------------------

        private void Handle_PlayerRestart()
        {
            // N_References._navMeshAgent.enabled = false;
            // transform.position = N_References._RestartPoint.position;
            // N_References._navMeshAgent.enabled = true;
            // N_References._detections.Set_PlayerFound(false);
        }

        private void Handle_PlayerDies()
        {

        }

        #endregion
    }


    public enum NPC_State_Type
    {
        None,
        Idle,
        Patrol,
        Chase_Target,
        Attack

    }

    public enum NPC_Summon_Type
    {
        None,
        Building,
        Car
    }
}
