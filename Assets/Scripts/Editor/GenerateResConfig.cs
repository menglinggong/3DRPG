using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/*
1.编译器代码：继承自Editor类，只需要在unity编译器中执行的代码
2.菜单项 特性 [MenuItem("****")]：用于修饰需要在unity编译器中产生菜单按钮的方法
3.AssetDatabase：包含了只适用于unity编译器中操作资源的相关功能
4.StreamingAssets：unity特殊目录之一，该目录中的文件不会被压缩，适合在移动端读取资源（在PC端还可以写入）
  持久化路径：Application.persistentDataPath（软件安装时才产生） 该路径可以在运行时进行读写操作，unity外部目录
 */

/// <summary>
/// 生成资源映射表
/// </summary>
public class GenerateResConfig : Editor
{
    [MenuItem("Tools/Resources/Generate ResConfig File")]
    public static void Generate()
    {
        //生成资源配置文件
        //1.查找Resources 目录下所有预制件完整路径
        //resFiles为GUID
        string[] resFiles = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/Resources" });

        for (int i = 0; i < resFiles.Length; i++)
        {
            resFiles[i] = AssetDatabase.GUIDToAssetPath(resFiles[i]);
            //2.生成对应关系 名称=路径
            string fileName = Path.GetFileNameWithoutExtension(resFiles[i]);
            //去除固定字符串
            string filePath = resFiles[i].Replace("Assets/Resources/", string.Empty).Replace(".prefab", string.Empty);

            resFiles[i] = fileName + "=" + filePath;
        }

        //3.写入文件
        File.WriteAllLines("Assets/StreamingAssets/ResConfig.txt", resFiles);
        //手动刷新工程
        AssetDatabase.Refresh();
    }
}