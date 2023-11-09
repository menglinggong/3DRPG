using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// ����״̬
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
            //���Ŵ�������
            //fsmBase.animator.SetBool();
        }

        public override void OnExitState(FSMBase fsmBase)
        {
            base.OnExitState(fsmBase);
            //ȡ�����Ŵ�������
            //fsmBase.animator.SetBool();
        }
    }
}