using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// ɱ��Ŀ������
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

