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
            foreach (var target in deployer.SkillData.attackTargets)
            {
                target.GetComponent<Animator>().SetTrigger("Dizzy");
            }
        }
    }
}

