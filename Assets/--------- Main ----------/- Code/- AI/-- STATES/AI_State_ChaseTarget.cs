using UnityEngine;
using UnityEngine.AI;

namespace Henmova.Vaibhav
{

    public class AI_State_ChaseTarget : NPC_Ai_StateBase
    {
        public AI_State_ChaseTarget(NPC_AI_M_Main main, NPC_Ai_StateFactory factory, NPC_State_Type stateType = NPC_State_Type.None) : base(main, factory, stateType)
        {
        }

        private NavMeshAgent _agent => N_Main.N_References._navMeshAgent;
        bool _isReached = false;
        int _lastIndex;
        private float _lastStopDistance;
        private Transform _lastTarget;

        public override void Enter()
        {
            _isReached = false;
            _agent.isStopped = false;
            _agent.speed = N_Main.N_References._ChaseSpeed;
            N_Main.N_References.Animator_SetRun();
            _lastStopDistance = _agent.stoppingDistance;
            _agent.ResetPath();
            _agent.stoppingDistance = 1.3f;

            // if attack target is not set, then return
            if (!N_Main.N_References._Player)
            {
                Debug.Log("No Attack Target Found");
                SwitchState(N_Factory.S_Idle);
                return;
            }
            N_Main._isChasing = true;

            // set destination to attack target
            SetDestination(N_Main.N_References._Player.position);

            // sound
            N_Main.N_References.Play_SoundChase();
            N_Main.N_References.Play_SoundFound();
            N_Main.N_References.Play_RunFoot(true);
        }

        public override void Exit()
        {
            N_Main.N_References.Play_RunFoot(false);
            _agent.stoppingDistance = _lastStopDistance;
            N_Main.Last_Paused_State = N_State_Type;
        }

        public override void Update()
        {
            // set destination to attack target
            SetDestination(N_Main.N_References._Player.position);
            N_Main.CurrentTarget_RemainingDistance = _agent.remainingDistance;
            N_Main.N_HasPath = _agent.pathPending;

            float distanceToTarget = Vector3.Distance(N_Main.N_References._Player.position, N_Main.transform.position);


            // if (distanceToTarget > N_Main.N_References._DistanceTo_StopChase)
            // {
            //     // If the target is too far away, switch to idle state
            //     SwitchState(N_Factory.S_Idle);
            //     Debug.Log("Player is out of reach, back to idle");
            //     return;
            // }

            // if agent is not moving, then set new destination
            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance && !_isReached)
            {

                _isReached = true;
                _lastTarget = null;
                Debug.Log("Player found for attack");
                if (!Game_Manager.instance.GameWon)
                    SwitchState(N_Factory.S_Attack);
                else
                    SwitchState(N_Factory.S_Idle);
            }
        }

        // set destination
        private void SetDestination(Vector3 destination)
        {
            _agent.SetDestination(destination);
        }
    }

}