using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������Ʒ����Ϣ
/// </summary>
public class ArticleInfo_Cloth : ArticleInfoBase
{
    /// <summary>
    /// �·�����
    /// </summary>
    public ClothKind ClothKind;
    /// <summary>
    /// �·��ĵȼ�
    /// </summary>
    public ClothLevel ClothLevel;
    /// <summary>
    /// ������
    /// </summary>
    public float Defense;
    /// <summary>
    /// ���ε�����Ч��
    /// </summary>
    public ClothEffect ClothEffect;
    /// <summary>
    /// ��������Ч���ȼ�
    /// </summary>
    public ClothEffectLevel ClothEffectLevel;
}

/// <summary>
/// ��������
/// </summary>
public enum ClothKind
{
    HeadMounted,            //ͷ��ʽ
    UpperOuterGarment,      //����
    Pant                    //����
}

/// <summary>
/// ���εĵȼ�
/// </summary>
public enum ClothLevel
{
    Default,
    One,
    Two,
    Three,
    Top
}

/// <summary>
/// ���ε�����Ч��
/// </summary>
public enum ClothEffect
{
    Default,            //������Ч��
    AttackUp,           //����������
    DefenceUp,          //����������
    SwimSpeedUp,        //��Ӿ�ٶ�����
    MoveSpeedUp,        //��������
    QuietnessUp,        //����������
    VigorExpendDown,    //�������ļ���
    ColdProof,          //����
    FireProof,          //����
    AntiElectricity,    //����
    HeatResisting,      //����
}

/// <summary>
/// ��������Ч���ȼ�
/// </summary>
public enum ClothEffectLevel
{
    Default,
    LevelOne,
    LevelTwo,
    LevelThree,
}
