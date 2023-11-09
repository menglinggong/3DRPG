using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// AI�����ļ���ȡ-����������
    /// </summary>
    public class AIConfigurationReaderFactory
    {
        //�洢�����Ѿ���ȡ-��������AI�����ļ���key���ļ�����value��AI�����ļ���ȡ-������
        private static Dictionary<string, AIConfigurationReader> cache = new Dictionary<string, AIConfigurationReader>();

        public static Dictionary<string, Dictionary<string, string>> GetMap(string fileName)
        {
            if (!cache.ContainsKey(fileName))
            {
                cache.Add(fileName, new AIConfigurationReader(fileName));
            }

            return cache[fileName].map;
        }
    }
}