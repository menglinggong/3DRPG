using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI.FSM
{
    /// <summary>
    /// ����״̬�Ļ��֮࣬�������״̬�̳д���
    /// </summary>
    public abstract class FSMState
    {
        /// <summary>
        /// ״̬�ı�ţ��ز�����--------------------------------------------------------1
        /// </summary>
        public FSMStateID StateID { get; set; }

        /// <summary>
        /// ������״̬��ӳ���--------------------------------------------------------------3
        /// </summary>
        private Dictionary<FSMTriggerID, FSMStateID> map;

        /// <summary>
        /// ������-------------------------------------------------------------------------4
        /// </summary>
        private List<FSMTrigger> Triggers;

        public FSMState()
        {
            map = new Dictionary<FSMTriggerID, FSMStateID>();

            Triggers = new List<FSMTrigger>();

            Init();
        }

        /// <summary>
        /// �жϵ�ǰ״̬���ĸ��������㣬�л�����һ��״̬
        /// </summary>
        public void Reason(FSMBase fsmBase)
        {
            foreach (var trigger in Triggers)
            {
                if (trigger.HandleTrigger(fsmBase))
                {
                    //�������㣬�л�״̬
                    FSMStateID stateID = map[trigger.TriggerID];
                    fsmBase.ChangeState(stateID);
                    return;
                }
            }
        }

        /// <summary>
        /// Ҫ����������ʼ��״̬��Ϊ��Ÿ�ֵ------------------------------------------------2
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// ��״̬�����ã����ӳ���������������----------------------------------------------5
        /// </summary>
        /// <param name="triggerID"></param>
        /// <param name="stateID"></param>
        public void AddMap(FSMTriggerID triggerID, FSMStateID stateID)
        {
            //���ӳ��
            map.Add(triggerID, stateID);
            //�����������������������
            AddTrigger(triggerID);
        }

        /// <summary>
        /// �����������������������-----------------------------------------------------------6
        /// </summary>
        /// <param name="triggerID"></param>
        private void AddTrigger(FSMTriggerID triggerID)
        {
            //ʹ�÷��䣬������������
            //����������淶��AI.FSM + triggerID + Trigger
            Type type = Type.GetType("AI.FSM." + triggerID + "Trigger");
            FSMTrigger trigger = Activator.CreateInstance(type) as FSMTrigger;
            Triggers.Add(trigger);
        }

        //Ϊ�����ṩ��ѡʵ��------------------------------------------------------------------------7

        // һ����״̬����õ�һЩ����
        // ��Щ��������д��״̬�����������������
        // Ҳ����д��һ���ض�������  public virtual void OnEnterState(******* ******);
        public virtual void OnEnterState(FSMBase fsmBase) { }

        public virtual void OnStayState(FSMBase fsmBase) { }

        public virtual void OnExitState(FSMBase fsmBase) { }
    }
}