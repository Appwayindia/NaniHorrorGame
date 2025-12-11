using Henmova.Vaibhav;
using Sirenix.OdinInspector;
using UnityEngine;
using Henmova.Hit;

namespace Henmova.Vaibhav
{
    public class NPC_Ai_StateFactory
    {

        public NPC_Ai_StateFactory(NPC_AI_M_Main main)
        {
            N_Main = main;
        }
        public NPC_AI_M_Main N_Main { get; private set; }

        [Title("--- States ---")]
        // Idle
        private AI_State_Idle _idle; public AI_State_Idle S_Idle
        {
            get
            {
                if (_idle == null) return _idle = new AI_State_Idle(N_Main, this, NPC_State_Type.Idle);
                return _idle;
            }
            private set => value = _idle;
        }

        // petrol
        private AI_State_Petrol _petrol; public AI_State_Petrol S_Petrol
        {
            get
            {
                if (_petrol == null) return _petrol = new AI_State_Petrol(N_Main, this, NPC_State_Type.Idle);
                return _petrol;
            }
            private set => value = _petrol;
        }

        // Chase target
        private AI_State_ChaseTarget _chaseTarget; public AI_State_ChaseTarget S_ChaseTarget
        {
            get
            {
                if (_chaseTarget == null) return _chaseTarget = new AI_State_ChaseTarget(N_Main, this, NPC_State_Type.Idle);
                return _chaseTarget;
            }
            private set => value = _chaseTarget;
        }

        // Attack
        private AI_State_Attack _attack; public AI_State_Attack S_Attack
        {
            get
            {
                if (_attack == null) return _attack = new AI_State_Attack(N_Main, this, NPC_State_Type.Idle);
                return _attack;
            }
            private set => value = _attack;
        }

        // Attack
        private AI_State_Stun _stun; public AI_State_Stun S_Stun
        {
            get
            {
                if (_stun == null) return _stun = new AI_State_Stun(N_Main, this, NPC_State_Type.Idle);
                return _stun;
            }
            private set => value = _stun;
        }

    }
}
