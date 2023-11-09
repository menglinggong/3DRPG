using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skill
{
    /// <summary>
    /// 扇形/圆形攻击选区
    /// </summary>
    public class SectorAttackSelector : IAttackSelector
    {
        public Transform[] GetTargets(SkillData skill, Transform skillTF)
        {
            //根据技能数据中的标签，获取所有目标
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

            //判断攻击范围（扇形/圆形）
            targets = targets.FindAll(t =>
                Vector3.Distance(t.position, skillTF.position) <= skill.attackDistance &&
                Vector3.Angle(skillTF.forward, t.position - skillTF.position) <= skill.attackAngle / 2);


            //筛选出活着的敌人(HP>0)
            targets = targets.FindAll(t => t.GetComponent<CharacterStats>().CharacterData.CurrentHealth > 0);

            //返回目标，判断技能是单攻还是群攻
            //如果是单攻则返回目标中距离最近的
            //skill.attackType

            if (skill.attackType == SkillAttackType.Group)
                return targets.ToArray();

            if (targets.Count == 0)
                return null;
            //判断单攻是，距离最近的目标
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