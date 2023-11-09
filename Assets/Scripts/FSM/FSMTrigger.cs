using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI.FSM
{
    /// <summary>
    /// ���������Ļ��֮࣬������������̳д���
    /// </summary>
    public abstract class FSMTrigger
    {
        /// <summary>
        /// �����ı��,�ز�����----------------------------1
        /// </summary>
        public FSMTriggerID TriggerID { get; set; }

        public FSMTrigger()
        {
            Init();
        }

        /// <summary>
        /// Ҫ����������ʼ��������Ϊ��Ÿ�ֵ--------------3
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// �߼������жϽ����----------------------------2
        /// һ�����ж��ǻ��õ�һЩ����
        /// ��Щ��������д��״̬�����������������
        /// Ҳ����д��һ���ض�������  public abstract bool HandleTrigger(******* ******);
        /// </summary>
        /// <returns></returns>
        public abstract bool HandleTrigger(FSMBase fsmBase);
    }
}