using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Common
{
    /// <summary>
    /// 配置文件读取器
    /// </summary>
    public class ConfigurationReader
    {
        /// <summary>
        /// 通过文件名称读取文件内容
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetConfigFile(string fileName)
        {
            string url;

            #region 分平台判断StreamingAssets路径
            //string url = "file://" + Application.streamingAssetsPath + "/" + fileName;
            //如果在编译器或单机中
            //if(Application.platform == RuntimePlatform.Android)
            //Unity宏标签
#if UNITY_EDITOR || UNITY_STANDALONE
            url = "file://" + Application.dataPath + "/StreamingAssets/" + fileName;
            //否则如果在Iphone下
#elif UNITY_IPHONE
            url = "file://" + Application.dataPath + "/Raw/" + fileName;
            //否则如果在android下
#elif UNITY_ANDROID
            url = "jar:file://" + Application.dataPath + "!/assets/" + fileName;
#endif
            #endregion
            WWW www = new WWW(url);
            while (true)
            {
                if (www.isDone)
                    return www.text;
            }
        }

        /// <summary>
        /// 解析文件内容
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="handler"></param>
        public static void Reader(string fileContent, Action<string> handler)
        {
            using (StringReader reader = new StringReader(fileContent))
            {
                //一行一行的读取
                //当内容不为空时，使用特定方法解析单行信息
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    handler(line);
                }
            }
        }
    }
}