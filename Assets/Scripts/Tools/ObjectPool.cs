using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// �����
/// </summary>
public class ObjectPool : ISingleton<ObjectPool>
{
    /// <summary>
    /// ���������
    /// </summary>
    Dictionary<string, List<GameObject>> pool = new Dictionary<string, List<GameObject>>();

    /// <summary>
    /// ��ȡ�����ʵ��
    /// </summary>
    /// <param name="templateName"></param>
    /// <returns></returns>
    public GameObject GetObject(string templateName, GameObject prefab)
    {
        GameObject go = null;
        if(!pool.ContainsKey(templateName))
        {
            pool.Add(templateName, new List<GameObject>());
        }

        if (pool[templateName].Count > 0)
        {
            go = pool[templateName][0];
            pool[templateName].Remove(go);
        }
        else
        {
            go = Instantiate(prefab);
            go.name = templateName;
        }

        return go;
    }

    /// <summary>
    /// �������������
    /// </summary>
    /// <param name="templateName"></param>
    public void ReleaseObject(string templateName, GameObject obj)
    {
        if (!pool.ContainsKey(templateName))
        {
            pool.Add(templateName, new List<GameObject>());
        }
        obj.transform.SetParent(this.transform);
        obj.SetActive(false);
        pool[templateName].Add(obj);
    }
}
