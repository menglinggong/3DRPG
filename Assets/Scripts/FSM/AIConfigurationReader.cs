using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System;

namespace AI.FSM
{
    /// <summary>
    /// AI�����ļ���ȡ-����
    /// </summary>
    public class AIConfigurationReader
    {
        //���ݽṹ
        //���ֵ�key����ǰ״̬��value��ӳ���
        //С�ֵ�key��������value��Ŀ��״̬
        public Dictionary<string, Dictionary<string, string>> map { get; private set; }
        //��ǰ״̬
        private string mainkey;

        public AIConfigurationReader(string fileName)
        {
            map = new Dictionary<string, Dictionary<string, string>>();
            //��ȡ�����ļ�
            string filecontent = ConfigurationReader.GetConfigFile(fileName);
            //���������ļ�
            ConfigurationReader.Reader(filecontent, BuildMap);
        }

        /// <summary>
        /// ���������ļ�
        /// </summary>
        /// <param name="line"></param>
        private void BuildMap(string line)
        {
            //ȥ���հף�����ǿ���/r/n����Ϊ���ַ�����
            line = line.Trim();
            if (string.IsNullOrEmpty(line))
                return;
            //�����[��ͷ
            if (line.StartsWith("["))
            {
                //[Idel]--->Idel
                mainkey = line.Substring(1, line.Length - 2);
                //���map
                map.Add(mainkey, new Dictionary<string, string>());
            }
            else
            {
                //ӳ��Nohealth>Dead
                string[] key_Value = line.Split('>');
                map[mainkey].Add(key_Value[0], key_Value[1]);
            }
        }
    }
}