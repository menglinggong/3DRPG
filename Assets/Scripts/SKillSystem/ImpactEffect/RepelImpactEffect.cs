using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Skill
{
    /// <summary>
    /// »÷ÍËÐ§¹û
    /// </summary>
    public class RepelImpactEffect : IImpactEffect
    {
        public void Execute(SkillDeployer deployer)
        {
            var owner = deployer.SkillData.owner;
            if (deployer.SkillData.attackTargets == null) return;
            foreach (var target in deployer.SkillData.attackTargets)
            {
                if(owner.transform.IsFacingTarget(target))
                {
                    Vector3 dir = (target.position - owner.transform.position).normalized;
                    //»÷ÍËÍæ¼Ò
                    NavMeshAgent targetAgent = target.GetComponent<NavMeshAgent>();
                    targetAgent.isStopped = true;
                    float force = deployer.SkillData.GetImpactypeByName("Repel").value;
                    targetAgent.velocity = dir * force;
                }
            }
        }
    }
}

