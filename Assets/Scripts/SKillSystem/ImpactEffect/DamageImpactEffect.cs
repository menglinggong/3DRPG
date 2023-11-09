using System.Collections;
using UnityEngine;

namespace RPG.Skill
{
    /// <summary>
    /// ����˺�
    /// </summary>
    public class DamageImpactEffect : IImpactEffect
    {
        //private SkillData skillData;

        /// <summary>
        /// ʵ������˺�Ч��
        /// </summary>
        /// <param name="deployer"></param>
        public void Execute(SkillDeployer deployer)
        {
            //skillData = deployer.SkillData;

            //������Щ�����ǳ����ģ������ظ�����˺�������ʹ��Э��дһ���ظ���Ѫ�ķ���
            //��Ϊ�ļ����벻�ǹ���ʵ���ϣ������޷�����Э�̣�����ʹ�þ���ļ����ͷ�������Э��
            //ֻ���г����˺��ļ���ʹ�ô˷������˴�Ϊ��ͨ�˺�
            //deployer.StartCoroutine(RepeatDamage(deployer));
            OnceDamage(deployer.SkillData);
        }

        /// <summary>
        /// �ظ�����˺�
        /// </summary>
        /// <returns></returns>
        private IEnumerator RepeatDamage(SkillDeployer deployer)
        {
            //��ʱ:�����Ѿ�������ʱ��
            float attackTime = 0;

            do
            {
                //��������˺�
                OnceDamage(deployer.SkillData);
                yield return new WaitForSeconds(deployer.SkillData.atkInterval);
                attackTime += deployer.SkillData.atkInterval;
                //��Ϊ���ܳ���ʱ����Ŀ����ܻ��뿪������Χ��������������Ŀ����Ҫʵʱ����
                deployer.CalculateTargets();

            } while (attackTime < deployer.SkillData.durationTime);//ʱ��С�ڼ��ܳ���ʱ�����ٴ�����˺�
        }

        /// <summary>
        /// ��������˺�
        /// </summary>
        private void OnceDamage(SkillData skillData)
        {
            if (skillData.attackTargets == null) return;

            foreach (var target in skillData.attackTargets)
            {
                //������ɵ��˺�������Ҫ��ϼ����ͷ��ߵĻ������������Ƽ�/������Ŀ��Ļ���/ħ�����ڶ�����
                //�˴�ֻ���򵥼���
                //�����ɫ״̬���д�����ϸ�Ŀ�Ѫ������ֻ��򵥼�����÷��������������
                //float damage = skillData.atkRatio * skillData.owner.GetComponent<CharacterStatus>().BaseAttack;
                //target.GetComponent<CharacterStatus>().ExpendHP(damage);

                float damage = skillData.owner.GetComponent<CharacterStats>().GetRealDamage();

                target.GetComponent<CharacterStats>().CharacterData.GetHurt(damage);

                //������Ϣ�����µ���Ѫ��
                EventManager.Instance.Invoke(MessageConst.UpdateHealth, target.GetComponent<EnemyController>().CharacterStats);
            }

            //����������Ч
        }
    }
}