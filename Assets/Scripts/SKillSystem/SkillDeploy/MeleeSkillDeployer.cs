using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skill
{
    /// <summary>
    /// ������ͷ����������ͷ���
    /// </summary>
    public class MeleeSkillDeployer : SkillDeployer
    {
        /// <summary>
        /// ������ͷŷ�ʽ
        /// </summary>
        public override void DeploySkill()
        {
            //ִ��ѡ���㷨
            CalculateTargets();
            //ִ��Ӱ���㷨
            ImpactTargets();
        }
    }
}