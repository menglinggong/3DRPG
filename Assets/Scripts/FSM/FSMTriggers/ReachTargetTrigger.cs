using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// 目标在攻击范围内条件
    /// </summary>
    public class ReachTargetTrigger : FSMTrigger
    {
        public override bool HandleTrigger(FSMBase fsmBase)
        {
            return fsmBase.EnemyController.TargetInAttackRange() || fsmBase.EnemyController.TargetInSkillRange();
        }

        public override void Init()
        {
            this.TriggerID = FSMTriggerID.ReachTarget;
        }
    }
}

