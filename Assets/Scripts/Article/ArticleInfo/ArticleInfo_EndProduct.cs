using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ʒ����Ʒ����Ϣ������ҩ����⿵�ʳ��
/// </summary>
public class ArticleInfo_EndProduct : ArticleInfoBase
{
    /// <summary>
    /// �ָ�Ѫ����ֵ
    /// </summary>
    public float HealthRecoverValue;

    /// <summary>
    /// ��Ʒ������Ч��
    /// </summary>
    public EndProductEffect Effect;

    /// <summary>
    /// ����Ч����ֵ
    /// </summary>
    public float EffectValue;

    /// <summary>
    /// ����Ч���ĳ���ʱ��
    /// </summary>
    public float EffectDuration;
}

/// <summary>
/// �ز�ӵ�е�����Ч��
/// </summary>
public enum EndProductEffect
{
    Default,                    //������Ч��
    AttackUp,                   //����������
    DefenceUp,                  //����������
    SwimSpeedUp,                //��Ӿ�ٶ�����
    MoveSpeedUp,                //��������
    QuietnessUp,                //����������
    ColdProof,                  //����
    FireProof,                  //����
    AntiElectricity,            //����
    HeatResisting,              //����
    HealthRecoverComplete,      //Ѫ����ȫ�ָ�
    VigoeRecover,               //�ָ�����
}
