using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skill
{
    /// <summary>
    /// 攻击选区接口
    /// 技能攻击的范围区域，例如诺手无情铁手的三角区域
    /// </summary>
    public interface IAttackSelector
    {
        /// <summary>
        /// 获取攻击区域内的目标
        /// </summary>
        /// <param name="skill">技能</param>
        /// <param name="SkillTF">技能的实体</param>
        /// <returns></returns>
        Transform[] GetTargets(SkillData skill, Transform skillTF);
    }
}