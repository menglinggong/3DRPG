using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// ×·»÷×´Ì¬
    /// </summary>
    public class ChaseState : FSMState
    {
        public override void Init()
        {
            this.StateID = FSMStateID.Chase;
        }

        public override void OnEnterState(FSMBase fsmBase)
        {
            base.OnEnterState(fsmBase);
            fsmBase.EnemyController.Chase();
        }

        public override void OnStayState(FSMBase fsmBase)
        {
            base.OnStayState(fsmBase);
            fsmBase.EnemyController.Chase();
        }

    }
}

