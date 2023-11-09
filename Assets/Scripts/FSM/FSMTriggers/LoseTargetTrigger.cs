using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// 丢失目标条件
    /// </summary>
    public class LoseTargetTrigger : FSMTrigger
    {
        public override bool HandleTrigger(FSMBase fsmBase)
        {
            return !fsmBase.EnemyController.FoundPlayer();
        }

        public override void Init()
        {
            this.TriggerID = FSMTriggerID.LoseTarget;
        }
    }
}

