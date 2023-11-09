using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI.FSM
{
    /// <summary>
    /// ������ö�٣��洢���е���������������ID
    /// ������������ı���������޸�
    /// </summary>
    public enum FSMTriggerID
    {
        //����ֵΪ0
        NoHealth,
        //����Ŀ��
        SawTarget,
        //Ŀ����빥����Χ
        ReachTarget,
        //��ʧĿ��
        LoseTarget,
        //���Ѳ��
        CompletePatrol,
        //��ɱĿ��
        KilledTarget,
        //Ŀ���뿪������Χ
        WithoutAttackRange,
        //.......
    }
}