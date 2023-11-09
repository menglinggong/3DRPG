using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// 待机状态
    /// </summary>
    public class IdelState : FSMState
    {
        public override void Init()
        {
            this.StateID = FSMStateID.Guard;
        }

        public override void OnEnterState(FSMBase fsmBase)
        {
            base.OnEnterState(fsmBase);
            //播放待机动画
            //fsmBase.animator.SetBool();
        }

        public override void OnExitState(FSMBase fsmBase)
        {
            base.OnExitState(fsmBase);
            //取消播放待机动画
            //fsmBase.animator.SetBool();
        }
    }
}