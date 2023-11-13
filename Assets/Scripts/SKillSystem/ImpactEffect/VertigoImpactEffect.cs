using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skill
{
    /// <summary>
    /// »÷ÔÎµÄÐ§¹û
    /// </summary>
    public class VertigoImpactEffect : IImpactEffect
    {
        public void Execute(SkillDeployer deployer)
        {
            if (deployer == null || deployer.SkillData == null || deployer.SkillData.attackTargets == null)
                return;

            foreach (var target in deployer.SkillData.attackTargets)
            {
                target.GetComponent<Animator>().SetTrigger("Dizzy");
            }
        }
    }
}

