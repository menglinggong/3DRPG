using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skill
{
    /// <summary>
    /// 技能释放器工厂类，生成算法对象
    /// </summary>
    public class DeployerconfigFactory
    {
        //缓存使用反射创建的对象
        private static Dictionary<string, object> cache = new Dictionary<string, object>();

        /// <summary>
        /// 生成技能区域对象
        /// 提供创建释放器各种算法对象的功能
        /// 作用：将对象的创建于使用分离，此处只创建，用还是在释放器内
        /// </summary>
        /// <param name="skillData"></param>
        /// <returns></returns>
        public static IAttackSelector CreateAttackSelector(SkillData skillData)
        {
            //创建算法对象

            //1.技能区域（圆形/矩形***）
            //skillData.selectorType
            //使用反射的方法
            //选区对象命名规则：
            //（命名空间名）RPG.Skill.+枚举名+AttackSelector
            //例如扇形选区：RPG.Skill.SectorAttackSelector
            string typeName = string.Format("RPG.Skill.{0}AttackSelector", skillData.selectorType);
            //Type type = Type.GetType(typeName);
            //selector = Activator.CreateInstance(type) as IAttackSelector;
            return CreateObject<IAttackSelector>(typeName);
        }

        /// <summary>
        /// 生成影响效果对象
        /// </summary>
        /// <param name="skillData"></param>
        /// <returns></returns>
        public static List<IImpactEffect> CreateImpactEffects(SkillData skillData)
        {
            //2.影响
            //skillData.impactype
            //同样使用反射的方法
            //影响的命名规范：
            //（命名空间名）RPG.Skill.+具体名称+ImpactEffect
            //例如蓝量的影响：RPG.Skill.CostSPImpactEffect
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
        /// 通过反射创建对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private static T CreateObject<T>(string typeName) where T : class
        {
            //由于反复使用反射创建对象消耗性能较高，所有使用缓存来解决此问题
            if (!cache.ContainsKey(typeName))
            {
                //Debug.LogError("反射");
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