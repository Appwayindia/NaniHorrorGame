using System.Collections;
using UnityEngine;

namespace Henmova.Vaibhav
{

    public class AI_State_Attack : NPC_Ai_StateBase
    {
        public AI_State_Attack(NPC_AI_M_Main main, NPC_Ai_StateFactory factory, NPC_State_Type stateType = NPC_State_Type.None) : base(main, factory, stateType)
        {
        }

        public override void Enter()
        {
            N_Main.N_References.Animator_SetAttack();
            N_Main.N_References.Animator_SetIdle();
            N_Main.N_References._navMeshAgent.isStopped = true;
            Vector3 attackPos = N_Main.N_References._Player.position;
            N_Main.transform.LookAt(new Vector3(attackPos.x, N_Main.transform.position.y, attackPos.z));

            ApplyDirectAttack();
            N_Main.N_References.Play_SoundCatch();
        }

        public override void Exit()
        {
            N_Main.N_References._navMeshAgent.isStopped = false;
        }

        private void ApplyDirectAttack()
        {
            Debug.Log("Attack successful!");
            N_Main.N_References._isGameOver = true;
            N_Main.N_References.Perform_Attack();
            Game_Manager.OnPlayerCaughtBy_Enemy?.Invoke();
        }
        

        public override void Update()
        {
            // float distanceToTarget = Vector3.Distance(N_Main.N_Attack_Target.position, N_Main.transform.position);

            // if (distanceToTarget > 40f)
            // {
            //     Debug.Log("Switching to idle because the Enemy target is far away");
            //     // If the target is too far away, switch to idle state
            //     SwitchState(N_Factory.S_Idle);
            // }

        }

    }

}