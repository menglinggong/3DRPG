using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skill
{
    /// <summary>
    /// ������ɵ�Ӱ��
    /// ���������������Ѫ��
    /// </summary>
    public interface IImpactEffect
    {
        /// <summary>
        /// Ӱ��Ч��
        /// ���磺�˺�����������HP��
        ///       �����˺����
        /// </summary>
        /// <param name="deployer">�ͷ����ű�</param>
        void Execute(SkillDeployer deployer);
    }
}