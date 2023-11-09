using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skill
{
    /// <summary>
    /// �����ͷ��������࣬�����㷨����
    /// </summary>
    public class DeployerconfigFactory
    {
        //����ʹ�÷��䴴���Ķ���
        private static Dictionary<string, object> cache = new Dictionary<string, object>();

        /// <summary>
        /// ���ɼ����������
        /// �ṩ�����ͷ��������㷨����Ĺ���
        /// ���ã�������Ĵ�����ʹ�÷��룬�˴�ֻ�������û������ͷ�����
        /// </summary>
        /// <param name="skillData"></param>
        /// <returns></returns>
        public static IAttackSelector CreateAttackSelector(SkillData skillData)
        {
            //�����㷨����

            //1.��������Բ��/����***��
            //skillData.selectorType
            //ʹ�÷���ķ���
            //ѡ��������������
            //�������ռ�����RPG.Skill.+ö����+AttackSelector
            //��������ѡ����RPG.Skill.SectorAttackSelector
            string typeName = string.Format("RPG.Skill.{0}AttackSelector", skillData.selectorType);
            //Type type = Type.GetType(typeName);
            //selector = Activator.CreateInstance(type) as IAttackSelector;
            return CreateObject<IAttackSelector>(typeName);
        }

        /// <summary>
        /// ����Ӱ��Ч������
        /// </summary>
        /// <param name="skillData"></param>
        /// <returns></returns>
        public static List<IImpactEffect> CreateImpactEffects(SkillData skillData)
        {
            //2.Ӱ��
            //skillData.impactype
            //ͬ��ʹ�÷���ķ���
            //Ӱ��������淶��
            //�������ռ�����RPG.Skill.+��������+ImpactEffect
            //����������Ӱ�죺RPG.Skill.CostSPImpactEffect
            List<IImpactEffect> impactEffects = new List<IImpactEffect>();

            foreach (var impactType in skillData.impactype)
            {
                string impactName = string.Format("RPG.Skill.{0}ImpactEffect", impactType);
                //Type impact = Type.GetType(name);
                //impactEffects.Add(Activator.CreateInstance(impact) as IImpactEffect);
                impactEffects.Add(CreateObject<IImpactEffect>(impactName));
            }

            return impactEffects;
        }


        /// <summary>
        /// ͨ�����䴴������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private static T CreateObject<T>(string typeName) where T : class
        {
            //���ڷ���ʹ�÷��䴴�������������ܽϸߣ�����ʹ�û��������������
            if (!cache.ContainsKey(typeName))
            {
                //Debug.LogError("����");
                Type type = Type.GetType(typeName);
                object instance = Activator.CreateInstance(type);
                cache.Add(typeName, instance);
            }
            return cache[typeName] as T;

            //Type type = Type.GetType(typeName);
            //return Activator.CreateInstance(type) as T;
        }
    }
}