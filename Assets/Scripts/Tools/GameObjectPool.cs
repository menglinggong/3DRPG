using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /*
    ʹ�÷�����
    1.����Ƶ������/���ٵ����壬��ͨ������ش���/����
        GameObject go = GameObjectPool.Instance.CreateObject("���", Ԥ����, λ��, ��ת);
        GameObjectPool.Instance.CollectObject(��Ϸ����);
    2.��Ҫͨ������ش��������壬�����Ҫÿ�δ���ʱִ��ĳЩ���������������������ű�ʵ��IAwakeAble�ӿ�
     */

    /// <summary>
    /// �����
    /// </summary>
    public class GameObjectPool : MonoSingleton<GameObjectPool>
    {
        /// <summary>
        /// ÿ����ʾ��ִ�У��ӿ�
        /// </summary>
        public interface IAwakeAble
        {
            void OnAwake();
        }

        //�����
        private Dictionary<string, List<GameObject>> cache;
        public override void Init()
        {
            base.Init();
            cache = new Dictionary<string, List<GameObject>>();
        }

        /// <summary>
        /// ͨ������أ���������
        /// </summary>
        /// <param name="key">���</param>
        /// <param name="prefab">Ԥ����</param>
        /// <param name="position">����λ��</param>
        /// <param name="rotation">������ת</param>
        /// <returns></returns>
        public GameObject CreateObject(string key, GameObject prefab, Vector3 position, Quaternion rotation)
        {
            GameObject go = null;
            //����ָ�����п���ʹ�õĶ���
            if (cache.ContainsKey(key))
                go = cache[key].Find(s => !s.activeInHierarchy);
            //����������û�ж����򴴽����󲢼������
            if (go == null)
            {
                go = Instantiate(prefab);
                //�������û��key������Ӽ�¼
                if (!cache.ContainsKey(key))
                    cache.Add(key, new List<GameObject>());
                cache[key].Add(go);
            }

            //ʹ�ö���
            go.transform.position = position;
            go.transform.rotation = rotation;
            go.SetActive(true);
            //�����󴴽�ʱ��Ҫִ��ĳЩ��������ķ��������Դ�ʱִ�У�����ʵ��OnAwake��������
            foreach (var awakwAble in go.GetComponents<IAwakeAble>())
            {
                awakwAble.OnAwake();
            }
            return go;
        }

        /// <summary>
        /// ���ն���
        /// </summary>
        /// <param name="go">��Ҫ�����յĶ���</param>
        /// <param name="delay">�ӳ�ʱ��</param>
        public void CollectObject(GameObject go, float delay = 0)
        {
            StartCoroutine(CollectObjectDelay(go, delay));
        }

        /// <summary>
        /// �ӳٻ��ն���
        /// </summary>
        /// <param name="go"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private IEnumerator CollectObjectDelay(GameObject go, float delay)
        {
            yield return new WaitForSeconds(delay);
            go.SetActive(false);
            go.transform.SetParent(transform);
        }

        /// <summary>
        /// ���key��Ӧ�Ķ��������
        /// </summary>
        /// <param name="key"></param>
        public void Clear(string key)
        {
            if (cache.ContainsKey(key))
            {
                foreach (var obj in cache[key])
                {
                    Destroy(obj);
                }

                cache.Remove(key);
            }
        }

        /// <summary>
        /// ��ն�������ж���
        /// </summary>
        public void ClearAll()
        {
            //ӦΪClear��������remove�Ƴ��ֵ�����ݣ�������Ҫ��һ����ʱ���б�洢keys
            //foreach (var key in new List<string>(cache.Keys))
            //{
            //    Clear(key);
            //}

            //Ч��ͬ��
            List<string> keys = new List<string>(cache.Keys);

            foreach (var key in keys)
            {
                Clear(key);
            }

            cache.Clear();
        }
    }
}