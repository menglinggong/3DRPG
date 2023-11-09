using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    //namespace ����.��Ŀ����.ģ��

    /// <summary>
    /// �ű�����������
    /// �ص㣺
    /// 1.Ψһ
    /// 2.����
    /// ���ܣ�
    /// 1.����һ����Ϸ����ĵ�һ����������ֻ����һ������
    /// 2.�Ե�һ��Ϸ����Ŀ��ƺ͵���
    /// ���ͣ�
    /// ������<T>,Ϊ��������Դ�������
    /// ����Լ�������Ͳ���������ָ���Ļ����������ָ���Ļ��ࡣ
    /// ������Ҫ�̳д˵�����
    /// ���ʹ�ã�
    /// 1.�̳�ʱ�����봫����������
    /// 2.������ű����������У�ͨ���������ͷ���Instance���ԣ� XXXX.Instance.XXX
    /// </summary>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        // T ���������� ͨ�� where T : MonoSingleton<T> ������
        //�������
        private static T instance;
        //ֻ�ܶ�ȡget
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    //�ڳ����и�����������������
                    //ִֻ��һ��
                    instance = FindObjectOfType<T>();
                    //������û�������==��Ϸ����δ���ؽű�
                    if (instance == null)
                    {
                        //����һ���ű���������ִ��Awake��
                        new GameObject("Singleton of " + typeof(T)).AddComponent<T>();
                    }
                    else
                    {
                        //����ҵ������࣬������ʼ��
                        instance.Init();
                    }
                }
                return instance;
            }
        }

        protected void Awake()
        {
            //�ű����й��ص���Ϸ�������ˣ���Awake
            if (instance == null)
            {
                //���� = ���� as��ǿת�ɣ� ����
                //����Լ�� Where T �� MonoSingleton<T>
                instance = this as T;
                Init();
            }
        }

        /// <summary>
        /// ��Ϊ�̳�Unity���಻���������캯��ʵ�����ģ�����Ҫ�½�һ��Init������ͨ����дOverrideʵ��һЩ�����ĳ�ʼ����
        /// �������ʼ��������Ҫ��awake�����г�ʼ��
        /// </summary>
        public virtual void Init()
        {

        }
    }
}