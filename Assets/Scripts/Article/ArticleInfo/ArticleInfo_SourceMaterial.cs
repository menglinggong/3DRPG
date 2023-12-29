using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ز�����Ʒ����Ϣ
/// TODO:���Ż�
/// </summary>
[Serializable]
public class ArticleInfo_SourceMaterial : ArticleInfoBase
{
    /// <summary>
    /// ӵ�е�����
    /// </summary>
    public int Count;

    /// <summary>
    /// �ָ�Ѫ����ֵ
    /// </summary>
    public float HealthRecoverValue;

    /// <summary>
    /// �زĵ�����Ч��
    /// </summary>
    public SourceMaterialEffect Effect;

    /// <summary>
    /// �ز�����Ч����ֵ
    /// </summary>
    public float EffectValue;

    /// <summary>
    /// ����Ч������ʱ��
    /// </summary>
    public float EffectDuration;

    public ArticleInfo_SourceMaterial(ArticleInfo_SourceMaterial info) : base(info)
    {
        this.Count = info.Count;
        this.HealthRecoverValue = info.HealthRecoverValue;
        this.Effect = info.Effect;
        this.EffectValue = info.EffectValue;
        this.EffectDuration = info.EffectDuration;
    }

    public ArticleInfo_SourceMaterial()
    {

    }

    public override ArticleInfoBase Copy()
    {
        ArticleInfo_SourceMaterial info = new ArticleInfo_SourceMaterial(this);
        return info;
    }
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
