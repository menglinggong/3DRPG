using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// Ê¤Àû×´Ì¬
    /// </summary>
    public class WinState : FSMState
    {
        public override void Init()
        {
            this.StateID = FSMStateID.Win;
        }

        public override void OnEnterState(FSMBase fsmBase)
        {
            base.OnEnterState(fsmBase);
            fsmBase.EnemyController.Win();
        }

        public override void OnStayState(FSMBase fsmBase)
        {
            base.OnStayState(fsmBase);
            fsmBase.EnemyController.Win();
        }
    }
}

