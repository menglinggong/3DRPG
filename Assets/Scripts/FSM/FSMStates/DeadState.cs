using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// ����״̬
    /// </summary>
    public class DeadState : FSMState
    {
        public override void Init()
        {
            this.StateID = FSMStateID.Dead;
        }

        /// <summary>
        /// ��������״̬
        /// </summary>
        /// <param name="fsmBase"></param>
        public override void OnEnterState(FSMBase fsmBase)
        {
            base.OnEnterState(fsmBase);
            //�����������Լ�hp���������������ط��Ѿ�ִ�У����ڲ��Ŷ������������£��缼��ϵͳ�еȣ�

            //����״̬��������״̬����updataһֱ���У�Ȼ��ĳ��������������������
            fsmBase.EnemyController.Dead();
            fsmBase.enabled = false;
        }
    }
}