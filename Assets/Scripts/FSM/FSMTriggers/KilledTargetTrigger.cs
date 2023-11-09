using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// 杀死目标条件
    /// </summary>
    public class KilledTargetTrigger : FSMTrigger
    {
        public override bool HandleTrigger(FSMBase fsmBase)
        {
            return fsmBase.EnemyController.IsPlayerDie;
        }

        public override void Init()
        {
            this.TriggerID = FSMTriggerID.KilledTarget;
        }
    }
}

