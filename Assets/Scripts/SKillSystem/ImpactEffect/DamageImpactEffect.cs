using System.Collections;
using UnityEngine;

namespace RPG.Skill
{
    /// <summary>
    /// 造成伤害
    /// </summary>
    public class DamageImpactEffect : IImpactEffect
    {
        //private SkillData skillData;

        /// <summary>
        /// 实现造成伤害效果
        /// </summary>
        /// <param name="deployer"></param>
        public void Execute(SkillDeployer deployer)
        {
            //skillData = deployer.SkillData;

            //由于有些技能是持续的，会多次重复造成伤害，所以使用协程写一个重复扣血的方法
            //因为文件代码不是挂在实体上，所以无法开启协程，所以使用具体的技能释放器开启协程
            //只有有持续伤害的技能使用此方法，此处为普通伤害
            //deployer.StartCoroutine(RepeatDamage(deployer));
            OnceDamage(deployer.SkillData);
        }

        /// <summary>
        /// 重复造成伤害
        /// </summary>
        /// <returns></returns>
        private IEnumerator RepeatDamage(SkillDeployer deployer)
        {
            //计时:攻击已经持续的时间
            float attackTime = 0;

            do
            {
                //单次造成伤害
                OnceDamage(deployer.SkillData);
                yield return new WaitForSeconds(deployer.SkillData.atkInterval);
                attackTime += deployer.SkillData.atkInterval;
                //因为技能持续时间内目标可能会离开攻击范围或者死亡，所以目标需要实时计算
                deployer.CalculateTargets();

            } while (attackTime < deployer.SkillData.durationTime);//时间小于技能持续时间则再次造成伤害
        }

        /// <summary>
        /// 单次造成伤害
        /// </summary>
        private void OnceDamage(SkillData skillData)
        {
            if (skillData.attackTargets == null) return;

            foreach (var target in skillData.attackTargets)
            {
                //计算造成的伤害量：需要结合技能释放者的基础攻击力，破甲/法穿，目标的护甲/魔抗等众多因素
                //此处只做简单计算
                //如果角色状态类中存在详细的扣血方法，只需简单计算出该方法所需参数即可
                //float damage = skillData.atkRatio * skillData.owner.GetComponent<CharacterStatus>().BaseAttack;
                //target.GetComponent<CharacterStatus>().ExpendHP(damage);

                float damage = skillData.owner.GetComponent<CharacterStats>().GetRealDamage();

                target.GetComponent<CharacterStats>().CharacterData.GetHurt(damage);

                //发送消息，更新敌人血条
                EventManager.Instance.Invoke(MessageConst.UpdateHealth, target.GetComponent<EnemyController>().CharacterStats);
            }

            //创建攻击特效
        }
    }
}