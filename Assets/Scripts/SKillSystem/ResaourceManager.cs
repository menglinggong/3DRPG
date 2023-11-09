using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 资源管理器
    /// </summary>
    public class ResourceManager
    {
        private static Dictionary<string, string> configMap;

        /// <summary>
        /// 作用：初始化类的静态数据成员
        /// 时机：类被加载时执行一次
        /// </summary>
        static ResourceManager()
        {
            //加载文件
            string fileContent = ConfigurationReader.GetConfigFile("ResConfig.txt");
            configMap = new Dictionary<string, string>();
            ConfigurationReader.Reader(fileContent, BuildMap);
        }

        /// <summary>
        /// 解析文件内容
        /// </summary>
        /// <param name="line">每一行的数据</param>
        private static void BuildMap(string line)
        {
            string[] key_Value = line.Split('=');
            configMap.Add(key_Value[0], key_Value[1]);
        }

        /// <summary>
        /// 加载资源
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