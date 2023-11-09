using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// AI配置文件读取-解析器工厂
    /// </summary>
    public class AIConfigurationReaderFactory
    {
        //存储所有已经读取-解析过的AI配置文件，key：文件名，value：AI配置文件读取-解析器
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