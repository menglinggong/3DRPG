using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skill
{
    /// <summary>
    /// ����������Ӱ��
    /// </summary>
    public class CostSPImpactEffect : IImpactEffect
    {
        /// <summary>
        /// ����Ӱ��Ч����ʵ��
        /// ����������Ӱ�죺������������
        /// </summary>
        /// <param name="deployer"></param>
        public void Execute(SkillDeployer deployer)
        {
            //��������
            deployer.SkillData.owner.GetComponent<CharacterStats>().CharacterData.ExpendSP(deployer.SkillData.costSP);
        }
    }
}