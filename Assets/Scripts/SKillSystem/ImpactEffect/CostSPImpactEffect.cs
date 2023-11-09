using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skill
{
    /// <summary>
    /// 消耗蓝量的影响
    /// </summary>
    public class CostSPImpactEffect : IImpactEffect
    {
        /// <summary>
        /// 具体影响效果的实现
        /// 消耗蓝量的影响：减少自身蓝量
        /// </summary>
        /// <param name="deployer"></param>
        public void Execute(SkillDeployer deployer)
        {
            //减少蓝量
            deployer.SkillData.owner.GetComponent<CharacterStats>().CharacterData.ExpendSP(deployer.SkillData.costSP);
        }
    }
}