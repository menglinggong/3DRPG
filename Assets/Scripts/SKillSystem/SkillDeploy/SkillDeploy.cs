using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skill
{
    /// <summary>
    /// �����ͷ���
    /// </summary>
    public abstract class SkillDeployer : MonoBehaviour
    {
        //��������
        private SkillData skillData;

        public SkillData SkillData
        {
            get
            {
                return skillData;
            }
            set
            {
                skillData = value;
                //�����㷨����
                InitDeployer();

            }
        }
        //ѡ��Ч������
        private IAttackSelector selector;
        //Ӱ��Ч������
        private List<IImpactEffect> impactEffects/* = new List<IImpactEffect>()*/;

        /// <summary>
        /// ��ʼ���ͷ���
        /// </summary>
        private void InitDeployer()
        {
            //�����㷨����
            //1.���������������Բ��/����***��
            selector = DeployerconfigFactory.CreateAttackSelector(skillData);
            //2.����Ӱ��Ч������
            impactEffects = DeployerconfigFactory.CreateImpactEffects(skillData);
        }

        //ִ���㷨����
        /// <summary>
        /// 1.ִ��ѡ���㷨
        /// ��ȡ����Ŀ�����
        /// </summary>
        public void CalculateTargets()
        {
            skillData.attackTargets = selector.GetTargets(skillData, this.transform);

            //foreach (var item in skillData.attackTargets)
            //{
            //    Debug.LogError(item.name);
            //}
        }

        /// <summary>
        /// 2.ִ��Ӱ���㷨
        /// ��ɼ���Ч��
        /// </summary>
        public void ImpactTargets()
        {
            foreach (var item in impactEffects)
            {
                //item.�ӿڷ���
                item.Execute(this);
            }
        }

        //�ͷŷ�ʽ
        /// <summary>
        /// �����ܹ��������ã�������ʵ�֣���������ͷŲ���
        /// </summary>
        public abstract void DeploySkill();
        
    }
}