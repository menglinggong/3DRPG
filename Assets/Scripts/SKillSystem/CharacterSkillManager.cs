using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System.Resources;

namespace RPG.Skill
{
    public class CharacterSkillManager : MonoBehaviour
    {
        public List<SkillData> skillDatas = new List<SkillData>();

        private void Start()
        {
            foreach (var data in skillDatas)
            {
                InitSkills(data);
            }
        }

        /// <summary>
        /// 初始化技能数据
        /// </summary>
        /// <param name="data"></param>
        private void InitSkills(SkillData data)
        {
            /*
            资源映射表
            资源名称       资源完整路径
            Test     =     Skill/Test
             */

            //由data.prefabName 生成data.skillPrefab
            //data.skillPrefab = Resources.Load<GameObject>("路径/" + data.prefabName);
            data.skillPrefab = Common.ResourceManager.Load<GameObject>(data.prefabName);
            data.owner = this.gameObject;
        }

        /// <summary>
        /// 准备技能，判断技能是否可以释放
        /// 技能释放条件：冷却结束+有多余蓝量
        /// </summary>
        /// <param name="skillID"></param>
        /// <returns></returns>
        public SkillData PrepareSkill(int skillID)
        {
            //根据id查找技能
            SkillData skill = skillDatas.Find(s => s.skillID == skillID);
            //判断条件
            if (skill != null && skill.coolRemain <= 0 && skill.costSP <= this.transform.GetComponent<CharacterStats>().CharacterData.SP)
                return skill;
            //返回技能数据
            else
                return null;
        }

        /// <summary>
        /// 生成技能
        /// </summary>
        public void GenerateSkill(SkillData data)
        {
            //创建技能预制体
            //GameObject skillObj = Instantiate(data.skillPrefab, transform.position, transform.rotation);
            if(data == null) return;
            GameObject skillObj = GameObjectPool.Instance.CreateObject(data.prefabName, data.skillPrefab, transform.position, transform.rotation);
            //播放攻击动画，此处为测试代码
            //this.transform.GetComponent<Animator>().SetTrigger("IsAttack");

            //传递技能数据
            SkillDeployer deployer = skillObj.GetComponent<SkillDeployer>();
            deployer.SkillData = data;
            deployer.DeploySkill();

            if(data.attackTargets != null && data.attackTargets.Length > 0)
                this.transform.LookAt(data.attackTargets[0]);
            //延迟销毁预制体
            //Destroy(skillObj, data.durationTime);
            GameObjectPool.Instance.CollectObject(skillObj, data.durationTime);
            //技能冷却 开启一个协程
            StartCoroutine(CoolTimeDown(data));
        }

        /// <summary>
        /// 计算技能冷却
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private IEnumerator CoolTimeDown(SkillData data)
        {
            //data.coolTime ---> data.coolRemain
            data.coolRemain = data.coolTime;
            while (data.coolRemain > 0)
            {
                yield return new WaitForSeconds(1);
                data.coolRemain--;
            }
        }
    }
}