using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI.FSM
{
    /// <summary>
    /// ״̬��ö�٣��洢���е�״̬������״̬ID
    /// �������״̬�ı���������޸�
    /// </summary>
    public enum FSMStateID
    {
        Default,
        //վ׮/�����������Լ�Ѳ�ߣ�
        Guard,
        //Ѳ��
        Patrol,
        //׷��
        Chase,
        //����
        Attack,
        //ʤ��
        Win,
        //����
        Dead
    }
}