using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/*
1.���������룺�̳���Editor�ֻ࣬��Ҫ��unity��������ִ�еĴ���
2.�˵��� ���� [MenuItem("****")]������������Ҫ��unity�������в����˵���ť�ķ���
3.AssetDatabase��������ֻ������unity�������в�����Դ����ع���
4.StreamingAssets��unity����Ŀ¼֮һ����Ŀ¼�е��ļ����ᱻѹ�����ʺ����ƶ��˶�ȡ��Դ����PC�˻�����д�룩
  �־û�·����Application.persistentDataPath�������װʱ�Ų����� ��·������������ʱ���ж�д������unity�ⲿĿ¼
 */

/// <summary>
/// ������Դӳ���
/// </summary>
public class GenerateResConfig : Editor
{
    [MenuItem("Tools/Resources/Generate ResConfig File")]
    public static void Generate()
    {
        //������Դ�����ļ�
        //1.����Resources Ŀ¼������Ԥ�Ƽ�����·��
        //resFilesΪGUID
        string[] resFiles = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/Resources" });

        for (int i = 0; i < resFiles.Length; i++)
        {
            resFiles[i] = AssetDatabase.GUIDToAssetPath(resFiles[i]);
            //2.���ɶ�Ӧ��ϵ ����=·��
            string fileName = Path.GetFileNameWithoutExtension(resFiles[i]);
            //ȥ���̶��ַ���
            string filePath = resFiles[i].Replace("Assets/Resources/", string.Empty).Replace(".prefab", string.Empty);

            resFiles[i] = fileName + "=" + filePath;
        }

        //3.д���ļ�
        File.WriteAllLines("Assets/StreamingAssets/ResConfig.txt", resFiles);
        //�ֶ�ˢ�¹���
        AssetDatabase.Refresh();
    }
}