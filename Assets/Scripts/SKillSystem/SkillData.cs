using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skill
{
    /// <summary>
    /// 技能数据类
    /// </summary>
    [Serializable]
    public class SkillData
    {
        /// <summary>
        /// 技能ID
        /// </summary>
        public int skillID;
        /// <summary>
        /// 技能名称
        /// </summary>
        public string name;
        /// <summary>
        /// 技能描述
        /// </summary>
        public string description;
        /// <summary>
        /// 冷却时间
        /// </summary>
        public float coolTime;
        /// <summary>
        /// 冷却剩余
        /// </summary>
        public float coolRemain;
        /// <summary>
        /// 法力值消耗
        /// </summary>
        public float costSP;
        /// <summary>
        /// 攻击距离
        /// </summary>
        public float attackDistance;
        /// <summary>
        /// 攻击角度
        /// </summary>
        public float attackAngle;
        /// <summary>
        /// 是否中断移动
        /// </summary>
        public bool IsInterruptMove = false;
        /// <summary>
        /// 攻击目标的Tags
        /// </summary>
        public string[] attackTargetTags = { "Enemy" };
        /// <summary>
        /// 攻击目标对象数组
        /// </summary>
        [HideInInspector]
        public Transform[] attackTargets;
        /// <summary>
        /// 技能影响类型
        /// </summary>
        public List<Impactype> impactypes;
        /// <summary>
        /// 连击的下一个技能编号
        /// </summary>
        public int nextBatterID;
        ///// <summary>
        ///// 伤害比率/伤害数值
        ///// </summary>
        //public float atkRatio;
        /// <summary>
        /// 持续时间
        /// </summary>
        public float durationTime;
        /// <summary>
        /// 伤害间隔时间
        /// </summary>
        public float atkInterval;
        /// <summary>
        /// 技能所属
        /// </summary>
        [HideInInspector]
        public GameObject owner;
        /// <summary>
        /// 技能预制体名称
        /// </summary>
        public string prefabName;
        /// <summary>
        /// 技能预制体对象
        /// </summary>
        [HideInInspector]
        public GameObject skillPrefab;
        /// <summary>
        /// 动画参数名称
        /// </summary>
        public string animationName;
        /// <summary>
        /// 动画是否是触发的
        /// </summary>
        public bool isTrigger = false;
        /// <summary>
        /// 受击特效名称
        /// </summary>
        public string hitFxName;
        /// <summary>
        /// 受击特效预制体
        /// </summary>
        [HideInInspector]
        public GameObject hitFxPrefab;
        /// <summary>
        /// 技能等级
        /// </summary>
        public int level;
        /// <summary>
        /// 攻击类型 单攻，群攻
        /// </summary>
        public SkillAttackType attackType;
        /// <summary>
        /// 技能范围类型 扇形，圆形，矩形......
        /// </summary>
        public SelectorType selectorType;

        //其他数据

        /// <summary>
        /// 通过影响效果的名称获取影响效果及其数值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Impactype GetImpactypeByName(string name)
        {
            return impactypes.Find(a =>a.name == name);
        }
    }

    /// <summary>
    /// 影响效果及其数值
    /// </summary>
    [Serializable]
    public class Impactype
    {
        /// <summary>
        /// 影响效果的名称
        /// </summary>
        public string name;
        /// <summary>
        /// 影响效果的值，不同类型的影响效果，值的意思不同
        /// 伤害--基础伤害值
        /// 击退--击退的力
        /// 眩晕--无意义，目前眩晕时长取决于动画，TODO:改为技能控制眩晕时长
        /// </summary>
        public float value;
    }
}