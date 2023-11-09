using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skill
{
    [RequireComponent(typeof(CharacterSkillManager))]//Ч����ʵ����Ӹýű����Զ����CharacterSkillManager�ű�
    /// <summary>
    /// ��ɫ����ϵͳ��
    /// ��װ����ϵͳ���ṩ�򵥵ļ����ͷŹ���
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
        /// ���ɼ���
        /// ������ʱ���ż��ܶ���ʱ����Ҫָ���ڲ���ĳһʱ�������ɼ��ܣ��˴��ṩ���ɼ��ܵķ�������϶����¼����ü���
        /// </summary>
        private void DeploySkill()
        {
            if (skillData == null)
                return;

            skillManager.GenerateSkill(skillData);
        }

        /// <summary>
        /// ʹ�ü��ܹ�����Ϊ����ṩ��
        /// </summary>
        /// <param name="skillID">����ID</param>
        /// <param name="isBatter">�Ƿ�����������Ҫ��ʵ�֣�Ҳ��û������</param>
        public void AttackUseSkill(int skillID, bool isBatter = false)
        {
            //���ü���IDΪ��ǰ���ܵ���һ����������ID
            if (skillData != null && isBatter)
                skillID = skillData.nextBatterID;

            //׼������
            skillData = skillManager.PrepareSkill(skillID);
            if (skillData == null) return;

            var info = animator.GetCurrentAnimatorStateInfo(0);
            if(info.IsName("Attack_Normal"))
            {
                //TODO:�ͷż���ʱ������ͨ����
                if (skillData.isTrigger)
                    animator.SetTrigger(skillData.animationName);
                else
                    animator.SetBool(skillData.animationName, true);

                animator.Play("Locomotion");
            }
            else if(info.IsName("Locomotion"))
            {
                //�����Ŷ���
                if (skillData.isTrigger)
                    animator.SetTrigger(skillData.animationName);
                else
                    animator.SetBool(skillData.animationName, true);
            }
            //TODO:�����ͷŹ����У��޷�ʹ���������ܣ��޷�ʹ����ͨ����


            //���ż��ܶ������˴�Ϊ���Դ���,��������ʵ�ʶ���ϵͳ�����ý����޸�


            //���ɼ��ܣ�������ʱ���ż��ܶ���ʱ��ָ���ڲ���ĳһʱ�������ɼ��ܣ�������Ҫ��϶����¼�ʵ�֣��˴�������
            //skillManager.GenerateSkill(skillData);
            //--ѡ������Ŀ���(�ɲ���������ʵ��Ҫ��)
            //  1.ѡ��Ŀ�꣬���ָ��ʱ���ȡ��ѡ��
            //  2.ѡ��AĿ�꣬���Զ�ȡ��ѡ��Ǯ��ѡ��BĿ�꣬����Ҫ�ֶ���AĿ��ȡ��ѡ��

        }



        /// <summary>
        /// ʹ��������ܹ�����ΪNPC�ṩ��
        /// </summary>
        public void UseRandomSkill()
        {
            //���� �ӹ���������ѡ����ļ���
            //--�Ȳ�������������жϼ����Ƿ�����ͷţ����ã����ܲ�������������ܲ����ͷţ�Ȼ���ٴβ�����������������ɲ����ͷţ����ѭ����
            //--��ɸѡ�����п����ͷŵļ��ܣ��ڲ����������

            var usableSkills = skillManager.skillDatas.FindAll(s => skillManager.PrepareSkill(s.skillID) != null);

            if (usableSkills.Count == 0) return;

            int index = Random.Range(0, usableSkills.Count);
            AttackUseSkill(usableSkills[index].skillID);
        }
    }
}