using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Common
{
    /// <summary>
    /// �����ļ���ȡ��
    /// </summary>
    public class ConfigurationReader
    {
        /// <summary>
        /// ͨ���ļ����ƶ�ȡ�ļ�����
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetConfigFile(string fileName)
        {
            string url;

            #region ��ƽ̨�ж�StreamingAssets·��
            //string url = "file://" + Application.streamingAssetsPath + "/" + fileName;
            //����ڱ������򵥻���
            //if(Application.platform == RuntimePlatform.Android)
            //Unity���ǩ
#if UNITY_EDITOR || UNITY_STANDALONE
            url = "file://" + Application.dataPath + "/StreamingAssets/" + fileName;
            //���������Iphone��
#elif UNITY_IPHONE
            url = "file://" + Application.dataPath + "/Raw/" + fileName;
            //���������android��
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
        /// �����ļ�����
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="handler"></param>
        public static void Reader(string fileContent, Action<string> handler)
        {
            using (StringReader reader = new StringReader(fileContent))
            {
                //һ��һ�еĶ�ȡ
                //�����ݲ�Ϊ��ʱ��ʹ���ض���������������Ϣ
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    handler(line);
                }
            }
        }
    }
}