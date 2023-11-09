using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// 发现目标条件
    /// </summary>
    public class SawTargetTrigger : FSMTrigger
    {
        /// <summary>
        /// 判断条件是否满足
        /// </summary>
        /// <param name="fsmBase"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool HandleTrigger(FSMBase fsmBase)
        {
            return fsmBase.EnemyController.FoundPlayer();
        }

        public override void Init()
        {
            this.TriggerID = FSMTriggerID.SawTarget;
        }
    }
}

