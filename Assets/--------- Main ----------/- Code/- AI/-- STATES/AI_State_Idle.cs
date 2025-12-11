using System.Collections;
using Henmova.Vaibhav;
using UnityEngine;

namespace Henmova.Vaibhav
{

    public class AI_State_Idle : NPC_Ai_StateBase
    {
        public AI_State_Idle(NPC_AI_M_Main main, NPC_Ai_StateFactory factory, NPC_State_Type stateType = NPC_State_Type.None) : base(main, factory, stateType)
        {
        }

        public override void Enter()
        {
            // stopping agent if it was moving
            N_Main._isChasing = false;
            N_Main.N_References._navMeshAgent.ResetPath();
            N_Main.N_References.Animator_SetIdle();
            // Delay Change State
            DelayChange();

            N_Main.N_References._detections.Set_PlayerFound(false);
        }

        public override void Exit()
        {
            N_Main.StopAllCoroutines(); // make this local coroutine
        }

        public override void Update()
        {

        }

        private void DelayChange()
        {
            N_Main.StartCoroutine(Delay_Petrol());
        }

        IEnumerator Delay_Petrol()
        {
            yield return new WaitForSeconds(N_Main.N_References.Get_RandomIdleTime);
            SwitchState(N_Factory.S_Petrol);
        }
    }

}