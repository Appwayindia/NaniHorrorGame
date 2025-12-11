using UnityEngine;

namespace Henmova.Vaibhav
{
    public abstract class NPC_Ai_StateBase
    {
        public NPC_Ai_StateBase(NPC_AI_M_Main main, NPC_Ai_StateFactory factory, NPC_State_Type stateType = NPC_State_Type.None)
        {
            N_Main = main;
            N_Factory = factory;
            N_State_Type = stateType;
        }

        public NPC_AI_M_Main N_Main { get; private set; }
        public NPC_Ai_StateFactory N_Factory { get; private set; }
        public NPC_State_Type N_State_Type { get; protected set; }

        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update();

        public void SwitchState(NPC_Ai_StateBase newState)
        {
            N_Main.Last_Paused_State = N_State_Type;
            // exit previous state
            Exit();

            // assign new state
            N_Main.CurrentState = newState;
            N_Main.State = newState.N_State_Type;

            // enter new state
            N_Main.CurrentState.Enter();
        }

        public void SwitchState()
        {

            N_Main.State = N_State_Type;

            // enter new state
            N_Main.CurrentState.Enter();
        }
    }

    public enum NPC_Type
    {
        None,
        People
    }
}
