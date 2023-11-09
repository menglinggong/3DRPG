using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skill
{
    [RequireComponent(typeof(CharacterSkillManager))]//效果：实体添加该脚本是自动添加CharacterSkillManager脚本
    /// <summary>
    /// 角色技能系统类
    /// 封装技能系统，提供简单的技能释放功能
    /// </summary>
    public class CharacterSkillSystem : MonoBehaviour
    {
        private CharacterSkillManager skillManager;
        private Animator animator;
        private SkillData skillData;

        private void Start()
        {
            skillManager = GetComponent<CharacterSkillManager>();
            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// 生成技能
        /// 由于有时播放技能动画时，需要指定在播到某一时刻在生成技能，此处提供生成技能的方法，结合动画事件调用即可
        /// </summary>
        private void DeploySkill()
        {
            if (skillData == null)
                return;

            skillManager.GenerateSkill(skillData);
        }

        /// <summary>
        /// 使用技能攻击（为玩家提供）
        /// </summary>
        /// <param name="skillID">技能ID</param>
        /// <param name="isBatter">是否连击，根据要求实现，也可没有连击</param>
        public void AttackUseSkill(int skillID, bool isBatter = false)
        {
            //设置技能ID为当前技能的下一个连击技能ID
            if (skillData != null && isBatter)
                skillID = skillData.nextBatterID;

            //准备技能
            skillData = skillManager.PrepareSkill(skillID);
            if (skillData == null) return;

            var info = animator.GetCurrentAnimatorStateInfo(0);
            if(info.IsName("Attack_Normal"))
            {
                //TODO:释放技能时会打断普通攻击
                if (skillData.isTrigger)
                    animator.SetTrigger(skillData.animationName);
                else
                    animator.SetBool(skillData.animationName, true);

                animator.Play("Locomotion");
            }
            else if(info.IsName("Locomotion"))
            {
                //允许播放动画
                if (skillData.isTrigger)
                    animator.SetTrigger(skillData.animationName);
                else
                    animator.SetBool(skillData.animationName, true);
            }
            //TODO:技能释放过程中，无法使用其他技能，无法使用普通攻击


            //播放技能动画，此处为测试代码,后续根据实际动画系统的设置进行修改


            //生成技能，由于有时播放技能动画时，指定在播到某一时刻在生成技能，所以需要结合动画事件实现，此处不生成
            //skillManager.GenerateSkill(skillData);
            //--选中锁定目标等(可不做，根据实际要求)
            //  1.选中目标，间隔指定时间后取消选中
            //  2.选中A目标，在自动取消选中钱又选中B目标，则需要手动将A目标取消选中

        }



        /// <summary>
        /// 使用随机技能攻击（为NPC提供）
        /// </summary>
        public void UseRandomSkill()
        {
            //需求 从管理器中挑选随机的技能
            //--先产生随机数，在判断技能是否可以释放（不好，可能产生的随机数技能不能释放，然后再次产生的随机数技能依旧不能释放，如此循环）
            //--先筛选出所有可以释放的技能，在产生随机数。

            var usableSkills = skillManager.skillDatas.FindAll(s => skillManager.PrepareSkill(s.skillID) != null);

            if (usableSkills.Count == 0) return;

            int index = Random.Range(0, usableSkills.Count);
            AttackUseSkill(usableSkills[index].skillID);
        }
    }
}