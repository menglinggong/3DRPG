using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skill
{
    /// <summary>
    /// 技能释放器
    /// </summary>
    public abstract class SkillDeployer : MonoBehaviour
    {
        //技能数据
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
                //创建算法对象
                InitDeployer();

            }
        }
        //选区效果对象
        private IAttackSelector selector;
        //影响效果对象
        private List<IImpactEffect> impactEffects/* = new List<IImpactEffect>()*/;

        /// <summary>
        /// 初始化释放器
        /// </summary>
        private void InitDeployer()
        {
            //创建算法对象
            //1.创建技能区域对象（圆形/矩形***）
            selector = DeployerconfigFactory.CreateAttackSelector(skillData);
            //2.创建影响效果对象
            impactEffects = DeployerconfigFactory.CreateImpactEffects(skillData);
        }

        //执行算法对象
        /// <summary>
        /// 1.执行选区算法
        /// 获取技能目标对象
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
        /// 2.执行影响算法
        /// 造成技能效果
        /// </summary>
        public void ImpactTargets()
        {
            foreach (var item in impactEffects)
            {
                //item.接口方法
                item.Execute(this);
            }
        }

        //释放方式
        /// <summary>
        /// 供技能管理器调用，有子类实现，定义具体释放策略
        /// </summary>
        public abstract void DeploySkill();
        
    }
}