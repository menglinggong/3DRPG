using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System;

namespace AI.FSM
{
    /// <summary>
    /// AI配置文件读取-解析
    /// </summary>
    public class AIConfigurationReader
    {
        //数据结构
        //大字典key：当前状态，value：映射表
        //小字典key：条件，value：目标状态
        public Dictionary<string, Dictionary<string, string>> map { get; private set; }
        //当前状态
        private string mainkey;

        public AIConfigurationReader(string fileName)
        {
            map = new Dictionary<string, Dictionary<string, string>>();
            //读取配置文件
            string filecontent = ConfigurationReader.GetConfigFile(fileName);
            //解析配置文件
            ConfigurationReader.Reader(filecontent, BuildMap);
        }

        /// <summary>
        /// 解析配置文件
        /// </summary>
        /// <param name="line"></param>
        private void BuildMap(string line)
        {
            //去除空白（如果是空行/r/n，则为空字符串）
            line = line.Trim();
            if (string.IsNullOrEmpty(line))
                return;
            //如果以[开头
            if (line.StartsWith("["))
            {
                //[Idel]--->Idel
                mainkey = line.Substring(1, line.Length - 2);
                //添加map
                map.Add(mainkey, new Dictionary<string, string>());
            }
            else
            {
                //映射Nohealth>Dead
                string[] key_Value = line.Split('>');
                map[mainkey].Add(key_Value[0], key_Value[1]);
            }
        }
    }
}