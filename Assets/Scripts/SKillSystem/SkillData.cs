using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skill
{
    /// <summary>
    /// ����������
    /// </summary>
    [Serializable]
    public class SkillData
    {
        /// <summary>
        /// ����ID
        /// </summary>
        public int skillID;
        /// <summary>
        /// ��������
        /// </summary>
        public string name;
        /// <summary>
        /// ��������
        /// </summary>
        public string description;
        /// <summary>
        /// ��ȴʱ��
        /// </summary>
        public float coolTime;
        /// <summary>
        /// ��ȴʣ��
        /// </summary>
        public float coolRemain;
        /// <summary>
        /// ����ֵ����
        /// </summary>
        public float costSP;
        /// <summary>
        /// ��������
        /// </summary>
        public float attackDistance;
        /// <summary>
        /// �����Ƕ�
        /// </summary>
        public float attackAngle;
        /// <summary>
        /// �Ƿ��ж��ƶ�
        /// </summary>
        public bool IsInterruptMove = false;
        /// <summary>
        /// ����Ŀ���Tags
        /// </summary>
        public string[] attackTargetTags = { "Enemy" };
        /// <summary>
        /// ����Ŀ���������
        /// </summary>
        [HideInInspector]
        public Transform[] attackTargets;
        /// <summary>
        /// ����Ӱ������
        /// </summary>
        public List<Impactype> impactypes;
        /// <summary>
        /// ��������һ�����ܱ��
        /// </summary>
        public int nextBatterID;
        ///// <summary>
        ///// �˺�����/�˺���ֵ
        ///// </summary>
        //public float atkRatio;
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public float durationTime;
        /// <summary>
        /// �˺����ʱ��
        /// </summary>
        public float atkInterval;
        /// <summary>
        /// ��������
        /// </summary>
        [HideInInspector]
        public GameObject owner;
        /// <summary>
        /// ����Ԥ��������
        /// </summary>
        public string prefabName;
        /// <summary>
        /// ����Ԥ�������
        /// </summary>
        [HideInInspector]
        public GameObject skillPrefab;
        /// <summary>
        /// ������������
        /// </summary>
        public string animationName;
        /// <summary>
        /// �����Ƿ��Ǵ�����
        /// </summary>
        public bool isTrigger = false;
        /// <summary>
        /// �ܻ���Ч����
        /// </summary>
        public string hitFxName;
        /// <summary>
        /// �ܻ���ЧԤ����
        /// </summary>
        [HideInInspector]
        public GameObject hitFxPrefab;
        /// <summary>
        /// ���ܵȼ�
        /// </summary>
        public int level;
        /// <summary>
        /// �������� ������Ⱥ��
        /// </summary>
        public SkillAttackType attackType;
        /// <summary>
        /// ���ܷ�Χ���� ���Σ�Բ�Σ�����......
        /// </summary>
        public SelectorType selectorType;

        //��������

        /// <summary>
        /// ͨ��Ӱ��Ч�������ƻ�ȡӰ��Ч��������ֵ
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Impactype GetImpactypeByName(string name)
        {
            return impactypes.Find(a =>a.name == name);
        }
    }

    /// <summary>
    /// Ӱ��Ч��������ֵ
    /// </summary>
    [Serializable]
    public class Impactype
    {
        /// <summary>
        /// Ӱ��Ч��������
        /// </summary>
        public string name;
        /// <summary>
        /// Ӱ��Ч����ֵ����ͬ���͵�Ӱ��Ч����ֵ����˼��ͬ
        /// �˺�--�����˺�ֵ
        /// ����--���˵���
        /// ѣ��--�����壬Ŀǰѣ��ʱ��ȡ���ڶ�����TODO:��Ϊ���ܿ���ѣ��ʱ��
        /// </summary>
        public float value;
    }
}