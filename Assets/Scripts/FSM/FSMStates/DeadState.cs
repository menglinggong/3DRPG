using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// 死亡状态
    /// </summary>
    public class DeadState : FSMState
    {
        public override void Init()
        {
            this.StateID = FSMStateID.Dead;
        }

        /// <summary>
        /// 进入死亡状态
        /// </summary>
        /// <param name="fsmBase"></param>
        public override void OnEnterState(FSMBase fsmBase)
        {
            base.OnEnterState(fsmBase);
            //若死亡动画以及hp这类数据在其他地方已经执行，则不在播放动画或做其他事（如技能系统中等）

            //禁用状态机，以免状态机的updata一直运行，然后某种条件满足出现意外情况
            fsmBase.EnemyController.Dead();
            fsmBase.enabled = false;
        }
    }
}