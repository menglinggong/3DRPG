using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skill
{
    /// <summary>
    /// ���ε�Ч��
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

