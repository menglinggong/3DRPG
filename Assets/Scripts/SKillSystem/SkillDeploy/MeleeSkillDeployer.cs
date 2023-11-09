using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skill
{
    /// <summary>
    /// 具体的释放器：近身释放器
    /// </summary>
    public class MeleeSkillDeployer : SkillDeployer
    {
        /// <summary>
        /// 具体的释放方式
        /// </summary>
        public override void DeploySkill()
        {
            //执行选区算法
            CalculateTargets();
            //执行影响算法
            ImpactTargets();
        }
    }
}