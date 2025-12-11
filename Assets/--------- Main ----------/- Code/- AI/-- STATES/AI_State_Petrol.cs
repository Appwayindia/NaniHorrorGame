using Henmova.Vaibhav;
using UnityEngine;
using UnityEngine.AI;

namespace Henmova.Vaibhav
{
    public class AI_State_Petrol : NPC_Ai_StateBase
    {
        public AI_State_Petrol(NPC_AI_M_Main main, NPC_Ai_StateFactory factory, NPC_State_Type stateType = NPC_State_Type.None) : base(main, factory, stateType)
        {
        }

        private NavMeshAgent _agent => N_Main.N_References._navMeshAgent;
        bool _isReached = false;
        int _lastIndex;
        private Transform _lastTarget;

        public override void Enter()
        {
            _isReached = false;
            _agent.isStopped = false;
            _agent.speed = N_Main.N_References._MoveSpeed;
            if (!_lastTarget)
            {
                _lastTarget = N_Main.N_References.Get_RandomUniqueWayPoint(_lastTarget);
            }

            SetDestination(_lastTarget.position);
            N_Main.N_References.Animator_SetWalk();
        }

        public override void Exit()
        {
            N_Main.Last_Paused_State = N_State_Type;
        }

        public override void Update()
        {
            N_Main.CurrentTarget_RemainingDistance = _agent.remainingDistance;
            N_Main.N_HasPath = _agent.pathPending;

            // if agent is not moving, then set new destination
            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance && !_isReached)
            {
                _isReached = true;
                _lastTarget = null;
                N_Main.N_References.PlayRandomAudio();
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