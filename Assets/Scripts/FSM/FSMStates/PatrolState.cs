using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM{
    /// <summary>
    /// Ñ²Âß×´Ì¬
    /// </summary>
    public class PatrolState : FSMState
    {
        private bool isChase;
        public override void Init()
        {
            this.StateID = FSMStateID.Patrol;
        }

        public override void OnEnterState(FSMBase fsmBase)
        {
            base.OnEnterState(fsmBase);
            fsmBase.EnemyController.EnterPatrol();
        }

        public override void OnStayState(FSMBase fsmBase)
        {
            base.OnStayState(fsmBase);
            fsmBase.EnemyController.Patrol();
        }

    }
}

