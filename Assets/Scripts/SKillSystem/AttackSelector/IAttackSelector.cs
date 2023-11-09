using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skill
{
    /// <summary>
    /// ����ѡ���ӿ�
    /// ���ܹ����ķ�Χ��������ŵ���������ֵ���������
    /// </summary>
    public interface IAttackSelector
    {
        /// <summary>
        /// ��ȡ���������ڵ�Ŀ��
        /// </summary>
        /// <param name="skill">����</param>
        /// <param name="SkillTF">���ܵ�ʵ��</param>
        /// <returns></returns>
        Transform[] GetTargets(SkillData skill, Transform skillTF);
    }
}