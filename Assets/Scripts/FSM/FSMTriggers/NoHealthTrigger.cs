using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// 血量为0的条件
    /// </summary>
    public class NoHealthTrigger : FSMTrigger
    {
        /// <summary>
        /// 判断条件是否满足
        /// </summary>
        /// <param name="fsmBase"></param>
        /// <returns></returns>
        public override bool HandleTrigger(FSMBase fsmBase)
        {
            //如果HP<=0则返回true，否则返回false
            return fsmBase.EnemyController.CharacterStats.CharacterData.CurrentHealth <= 0;
        }

        public override void Init()
        {
            this.TriggerID = FSMTriggerID.NoHealth;
        }
    }
}