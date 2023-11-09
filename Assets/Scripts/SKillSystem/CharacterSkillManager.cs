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
        /// ��ʼ����������
        /// </summary>
        /// <param name="data"></param>
        private void InitSkills(SkillData data)
        {
            /*
            ��Դӳ���
            ��Դ����       ��Դ����·��
            Test     =     Skill/Test
             */

            //��data.prefabName ����data.skillPrefab
            //data.skillPrefab = Resources.Load<GameObject>("·��/" + data.prefabName);
            data.skillPrefab = Common.ResourceManager.Load<GameObject>(data.prefabName);
            data.owner = this.gameObject;
        }

        /// <summary>
        /// ׼�����ܣ��жϼ����Ƿ�����ͷ�
        /// �����ͷ���������ȴ����+�ж�������
        /// </summary>
        /// <param name="skillID"></param>
        /// <returns></returns>
        public SkillData PrepareSkill(int skillID)
        {
            //����id���Ҽ���
            SkillData skill = skillDatas.Find(s => s.skillID == skillID);
            //�ж�����
            if (skill != null && skill.coolRemain <= 0 && skill.costSP <= this.transform.GetComponent<CharacterStats>().CharacterData.SP)
                return skill;
            //���ؼ�������
            else
                return null;
        }

        /// <summary>
        /// ���ɼ���
        /// </summary>
        public void GenerateSkill(SkillData data)
        {
            //��������Ԥ����
            //GameObject skillObj = Instantiate(data.skillPrefab, transform.position, transform.rotation);
            if(data == null) return;
            GameObject skillObj = GameObjectPool.Instance.CreateObject(data.prefabName, data.skillPrefab, transform.position, transform.rotation);
            //���Ź����������˴�Ϊ���Դ���
            //this.transform.GetComponent<Animator>().SetTrigger("IsAttack");

            //���ݼ�������
            SkillDeployer deployer = skillObj.GetComponent<SkillDeployer>();
            deployer.SkillData = data;
            deployer.DeploySkill();

            if(data.attackTargets != null && data.attackTargets.Length > 0)
                this.transform.LookAt(data.attackTargets[0]);
            //�ӳ�����Ԥ����
            //Destroy(skillObj, data.durationTime);
            GameObjectPool.Instance.CollectObject(skillObj, data.durationTime);
            //������ȴ ����һ��Э��
            StartCoroutine(CoolTimeDown(data));
        }

        /// <summary>
        /// ���㼼����ȴ
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