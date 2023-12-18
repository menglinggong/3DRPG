using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

namespace RPG.Skill
{
    [RequireComponent(typeof(CharacterSkillManager))]//Ч����ʵ����Ӹýű����Զ����CharacterSkillManager�ű�
    /// <summary>
    /// ��ɫ����ϵͳ��
    /// ��װ����ϵͳ���ṩ�򵥵ļ����ͷŹ���
    /// </summary>
    public class CharacterSkillSystem : MonoBehaviour
    {
        private CharacterStats characterStats;
        private CharacterSkillManager skillManager;
        private Animator animator;
        private SkillData skillData;
        private Coroutine turnRoundCoroutine;

        private void Start()
        {
            skillManager = GetComponent<CharacterSkillManager>();
            animator = GetComponent<Animator>();
            characterStats = GetComponent<CharacterStats>();
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

            //�ͷż���ʱ����ҳ�����귽��
            LookAtMousePos();
            //�ͷż���ʱ���ж��Ƿ��ж��ƶ�
            InterruptMove(skillData);
            //�Ƿ��жϹ���
            InterruptAttack();
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

            //׼������
            skillData = skillManager.PrepareSkill(index);
            if (skillData == null) return;

            //�ͷż���ʱ���ж��Ƿ��ж��ƶ�
            InterruptMove(skillData);
            //�Ƿ��жϹ���
            InterruptAttack();
            //AttackUseSkill(usableSkills[index].skillID);
        }


        /// <summary>
        /// �Ƿ��жϹ���
        /// </summary>
        private void InterruptAttack()
        {
            var info = animator.GetCurrentAnimatorStateInfo(1);
            if (info.IsName("Attack_Normal"))
            {
                //��ͨ����״̬���ͷż��ܻ��ϵ�ǰ״̬����ͨ���������ϵ�ǰ״̬
                if (skillData.isTrigger)
                    animator.SetTrigger(skillData.animationName);
                else
                    animator.SetBool(skillData.animationName, true);

                animator.Play("Locomotion");
            }
            else if (info.IsName("Base State") || info.IsName("IdleBattle") || info.IsName("RunFWD"))
            {
                //�ǹ���״̬�������ż��ܶ���
                if (skillData.isTrigger)
                    animator.SetTrigger(skillData.animationName);
                else
                    animator.SetBool(skillData.animationName, true);
            }
            else
            {
                //�����ͷ�״̬���޷�ʹ���������ܣ��޷�ʹ����ͨ����
            }
        }

        /// <summary>
        /// �Ƿ��ж��ƶ�
        /// </summary>
        /// <param name="data"></param>
        private void InterruptMove(SkillData data)
        {
            if (data.IsInterruptMove)
            {
                //�ж��ƶ�
                NavMeshAgent agent = data.owner.GetComponent<NavMeshAgent>();
                agent.destination = data.owner.transform.position;
                agent.isStopped = true;
            }
        }

        /// <summary>
        /// ��ҿ������λ��
        /// </summary>
        private void LookAtMousePos()
        {
            //Vector3 pos = MouseManager.Instance.MousePosToWorld();
            //if (pos == Vector3.one * 10000)
            //    return;

            //if(turnRoundCoroutine != null)
            //    StopCoroutine(turnRoundCoroutine);

            //turnRoundCoroutine = StartCoroutine(transform.TurnRound(pos, characterStats.CharacterData.TurnRoundSpeed));
        }
    }
}