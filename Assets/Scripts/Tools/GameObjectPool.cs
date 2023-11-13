using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /*
    使用方法：
    1.所有频繁创建/销毁的物体，都通过对象池创建/回收
        GameObject go = GameObjectPool.Instance.CreateObject("类别", 预制体, 位置, 旋转);
        GameObjectPool.Instance.CollectObject(游戏物体);
    2.需要通过对象池创建的物体，如果需要每次创建时执行某些计算或其他操作，则让其脚本实现IAwakeAble接口
     */

    /// <summary>
    /// 对象池
    /// </summary>
    public class GameObjectPool : MonoSingleton<GameObjectPool>
    {
        /// <summary>
        /// 每次显示可执行，接口
        /// </summary>
        public interface IAwakeAble
        {
            void OnAwake();
        }

        //对象池
        private Dictionary<string, List<GameObject>> cache;
        public override void Init()
        {
            base.Init();
            cache = new Dictionary<string, List<GameObject>>();
        }

        /// <summary>
        /// 通过对象池，创建对象
        /// </summary>
        /// <param name="key">类别</param>
        /// <param name="prefab">预制体</param>
        /// <param name="position">物体位置</param>
        /// <param name="rotation">物体旋转</param>
        /// <returns></returns>
        public GameObject CreateObject(string key, GameObject prefab, Vector3 position, Quaternion rotation)
        {
            GameObject go = null;
            //查找指定类中可以使用的对象
            if (cache.ContainsKey(key))
                go = cache[key].Find(s => !s.activeInHierarchy);
            //如果对象池中没有对象，则创建对象并加入池中
            if (go == null)
            {
                go = Instantiate(prefab);
                //如果池中没有key，则添加记录
                if (!cache.ContainsKey(key))
                    cache.Add(key, new List<GameObject>());
                cache[key].Add(go);
            }

            //使用对象
            go.transform.position = position;
            go.transform.rotation = rotation;
            go.SetActive(true);
            //当对象创建时需要执行某些计算子类的方法，可以此时执行，对象实现OnAwake方法即可
            foreach (var awakwAble in go.GetComponents<IAwakeAble>())
            {
                awakwAble.OnAwake();
            }
            return go;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="go">需要被回收的对象</param>
        /// <param name="delay">延迟时间</param>
        public void CollectObject(GameObject go, float delay = 0)
        {
            StartCoroutine(CollectObjectDelay(go, delay));
        }

        /// <summary>
        /// 延迟回收对象
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
        /// 清空key对应的对象池数据
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
        /// 清空对象池所有对象
        /// </summary>
        public void ClearAll()
        {
            //应为Clear方法内有remove移除字典的数据，所以需要用一个临时的列表存储keys
            //foreach (var key in new List<string>(cache.Keys))
            //{
            //    Clear(key);
            //}

            //效果同上
            List<string> keys = new List<string>(cache.Keys);

            foreach (var key in keys)
            {
                Clear(key);
            }

            cache.Clear();
        }
    }
}