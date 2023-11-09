using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// ��Դ������
    /// </summary>
    public class ResourceManager
    {
        private static Dictionary<string, string> configMap;

        /// <summary>
        /// ���ã���ʼ����ľ�̬���ݳ�Ա
        /// ʱ�����౻����ʱִ��һ��
        /// </summary>
        static ResourceManager()
        {
            //�����ļ�
            string fileContent = ConfigurationReader.GetConfigFile("ResConfig.txt");
            configMap = new Dictionary<string, string>();
            ConfigurationReader.Reader(fileContent, BuildMap);
        }

        /// <summary>
        /// �����ļ�����
        /// </summary>
        /// <param name="line">ÿһ�е�����</param>
        private static void BuildMap(string line)
        {
            string[] key_Value = line.Split('=');
            configMap.Add(key_Value[0], key_Value[1]);
        }

        /// <summary>
        /// ������Դ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefabName"></param>
        /// <returns></returns>
        public static T Load<T>(string prefabName) where T : Object
        {
            //prefabName --> prefabPath
            return Resources.Load<T>(configMap[prefabName]);
        }
    }
}