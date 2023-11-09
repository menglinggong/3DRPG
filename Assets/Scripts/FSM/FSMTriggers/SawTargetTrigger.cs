using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// ����Ŀ������
    /// </summary>
    public class SawTargetTrigger : FSMTrigger
    {
        /// <summary>
        /// �ж������Ƿ�����
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

