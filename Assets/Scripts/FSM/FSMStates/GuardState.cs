using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// ÊØÎÀ×´Ì¬
    /// </summary>
    public class GuardState : FSMState
    {
        public override void Init()
        {
            this.StateID = FSMStateID.Guard;
        }

        public override void OnEnterState(FSMBase fsmBase)
        {
            base.OnEnterState(fsmBase);
            fsmBase.EnemyController.Guard();
        }

        public override void OnStayState(FSMBase fsmBase)
        {
            base.OnStayState(fsmBase);
            fsmBase.EnemyController.Guard();
        }
    }
}

