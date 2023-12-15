using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ز�����Ʒ����Ϣ
/// TODO:���Ż�
/// </summary>
public class ArticleInfo_SourceMaterial : ArticleInfoBase
{
    /// <summary>
    /// ӵ�е�����
    /// </summary>
    public int Count;

    /// <summary>
    /// �Ƿ����ֱ��ʹ��
    /// </summary>
    public bool IsCanDirectUse;

    /// <summary>
    /// �ָ�Ѫ����ֵ
    /// </summary>
    public float HealthRecoverValue;
}

/// <summary>
/// �ز�ӵ�е�����Ч��
/// </summary>
public enum SourceMaterialEffect
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
    SourceMaterialEffectUp,     //�زĵ�����Ч������
    HealthRecoverComplete,      //Ѫ����ȫ�ָ�
    VigoeRecover,               //�ָ�����
}
