using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// ¹¥»÷×´Ì¬
    /// </summary>
    public class AttackState : FSMState
    {
        public override void Init()
        {
            this.StateID = FSMStateID.Attack;
        }

        public override void OnEnterState(FSMBase fsmBase)
        {
            base.OnEnterState(fsmBase);
            fsmBase.EnemyController.AttackState();
        }

        public override void OnStayState(FSMBase fsmBase)
        {
            base.OnStayState(fsmBase);
            fsmBase.EnemyController.AttackState();
        }
    }
}

