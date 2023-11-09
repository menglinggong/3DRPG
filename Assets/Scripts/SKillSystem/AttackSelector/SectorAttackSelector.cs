using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skill
{
    /// <summary>
    /// ����/Բ�ι���ѡ��
    /// </summary>
    public class SectorAttackSelector : IAttackSelector
    {
        public Transform[] GetTargets(SkillData skill, Transform skillTF)
        {
            //���ݼ��������еı�ǩ����ȡ����Ŀ��
            //skill.attackTargetTags
            //string[] --> Transform[]

            List<Transform> targets = new List<Transform>();
            foreach (var tag in skill.attackTargetTags)
            {
                List<Transform> transforms = new List<Transform>();

                foreach (var item in GameObject.FindGameObjectsWithTag(tag))
                {
                    transforms.Add(item.transform);
                }

                targets.AddRange(transforms);
            }

            //�жϹ�����Χ������/Բ�Σ�
            targets = targets.FindAll(t =>
                Vector3.Distance(t.position, skillTF.position) <= skill.attackDistance &&
                Vector3.Angle(skillTF.forward, t.position - skillTF.position) <= skill.attackAngle / 2);


            //ɸѡ�����ŵĵ���(HP>0)
            targets = targets.FindAll(t => t.GetComponent<CharacterStats>().CharacterData.CurrentHealth > 0);

            //����Ŀ�꣬�жϼ����ǵ�������Ⱥ��
            //����ǵ����򷵻�Ŀ���о��������
            //skill.attackType

            if (skill.attackType == SkillAttackType.Group)
                return targets.ToArray();

            if (targets.Count == 0)
                return null;
            //�жϵ����ǣ����������Ŀ��
            float min = Vector3.Distance(skillTF.position, targets[0].position);
            Transform value = null;
            foreach (var target in targets)
            {
                float dis = Vector3.Distance(skillTF.position, target.position);
                if (dis <= min)
                {
                    min = dis;
                    value = target;
                }
            }

            return new Transform[] { value };
        }

    }
}