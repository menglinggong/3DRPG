using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// Ѫ��Ϊ0������
    /// </summary>
    public class NoHealthTrigger : FSMTrigger
    {
        /// <summary>
        /// �ж������Ƿ�����
        /// </summary>
        /// <param name="fsmBase"></param>
        /// <returns></returns>
        public override bool HandleTrigger(FSMBase fsmBase)
        {
            //���HP<=0�򷵻�true�����򷵻�false
            return fsmBase.EnemyController.CharacterStats.CharacterData.CurrentHealth <= 0;
        }

        public override void Init()
        {
            this.TriggerID = FSMTriggerID.NoHealth;
        }
    }
}