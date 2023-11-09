using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skill
{
    /// <summary>
    /// 技能造成的影响
    /// 例如减少蓝量，掉血等
    /// </summary>
    public interface IImpactEffect
    {
        /// <summary>
        /// 影响效果
        /// 例如：伤害敌人生命（HP）
        ///       可能伤害多次
        /// </summary>
        /// <param name="deployer">释放器脚本</param>
        void Execute(SkillDeployer deployer);
    }
}