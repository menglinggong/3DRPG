using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// ״̬��
    /// </summary>
    public class FSMBase : MonoBehaviour
    {
        /// <summary>
        /// ���˵Ŀ�����
        /// </summary>
        private EnemyController enemyController;

        public EnemyController EnemyController
        {
            get
            {
                return enemyController;
            }
        }

        #region ״̬����������
        /// <summary>
        /// ״̬�б�
        /// </summary>
        private List<FSMState> states;

        /// <summary>
        /// ��ǰ״̬
        /// </summary>
        private FSMState currentState;

        /// <summary>
        /// Ĭ��״̬
        /// </summary>
        private FSMState defaultState;

        [Tooltip("Ĭ�ϳ�ʼ״̬")]
        public FSMStateID defaultStateID;

        /// <summary>
        /// �ļ����ƣ���streamingAssets�£�
        /// </summary>
        public string FileName;

        private void Start()
        {
            ConfigFSM();
            enemyController = this.gameObject.GetComponent<EnemyController>();
            InitDefalutState();
        }

        /// <summary>
        /// ����״̬��
        /// </summary>
        private void ConfigFSM()
        {
            states = new List<FSMState>();
            //1.����״̬���󣨵���FSMState.AddMap��2.���õ�ǰ״̬
            //#region �ɵķ�����ͨ����д��ӣ���״̬����������ӻ��޸���Ҫ�޸Ĵ˴��룬����
            //���о���״̬�����IdelState��DeadState��ִ��
            //����״̬����
            ////IdelState idel = new IdelState();
            //����FSMState.AddMap ���״̬��������ӳ�䣨�м���ӳ����Ӽ��Σ�
            ////idel.AddMap(FSMTriggerID.NoHealth, FSMStateID.Dead);
            //idel.AddMap(FSMTriggerID.SawTarget)
            //����״̬���뵽״̬����״̬�б���
            ////states.Add(idel);

            //�������״̬�߼�������һ��
            ////DeadState dead = new DeadState();
            //����������û������״̬����û��ӳ�䲻��AddMap
            ////states.Add(dead);
            //������״̬�������
            //#endregion
            #region �µķ�����ͨ����ȡ�����ļ���ӣ���״̬����������ӻ��޸�ʱֻ���޸������ļ����ɣ����벻���޸�
            //�����NPCʹ��һ���������ļ�ʱ����ʱ���ظ�������ͬ�ļ������������ģ����Կ�����һ���·���
            //var map = new AIConfigurationReader(fileName).map;
            //�·�����ʹ�ù����洢��������ai�ļ�
            //StringBuilder path = new StringBuilder(Application.streamingAssetsPath);
            //path.Append("/");
            //path.Append(FilePath);
            
            var map = AIConfigurationReaderFactory.GetMap(FileName);
            //Application.streamingAssetsPath
            foreach (var stateName in map)
            {
                //stateName.key -->��ǰ״̬
                //stateName.value--->ӳ��
                Type type = Type.GetType("AI.FSM." + stateName.Key + "State");
                FSMState state = Activator.CreateInstance(type) as FSMState;

                foreach (var values in stateName.Value)
                {
                    //values.key -->�������
                    //values.value --->״̬���
                    //string ת enum
                    FSMTriggerID triggerID = (FSMTriggerID)Enum.Parse(typeof(FSMTriggerID), values.Key);
                    FSMStateID stateID = (FSMStateID)Enum.Parse(typeof(FSMStateID), values.Value);
                    //���ӳ��
                    state.AddMap(triggerID, stateID);
                }
                //���״̬
                states.Add(state);
            }
            #endregion
        }

        /// <summary>
        /// ��ʼ����ǰ״̬
        /// </summary>
        private void InitDefalutState()
        {
            defaultState = states.Find(s => s.StateID == defaultStateID);
            currentState = defaultState;
            //ִ�г�ʼ״̬�Ľ���
            currentState.OnEnterState(this);
        }

        /// <summary>
        /// ÿ֡������߼�
        /// </summary>
        public void Update()
        {
            if (currentState == null) return;

            //�жϵ�ǰ״̬������
            currentState.Reason(this);
            //ִ�е�ǰ״̬���߼�
            currentState.OnStayState(this);
        }

        /// <summary>
        /// �л�״̬
        /// </summary>
        /// <param name="stateID"></param>
        public void ChangeState(FSMStateID stateID)
        {
            //ִ����һ��״̬���˳�
            currentState.OnExitState(this);

            //���õ�ǰ״̬���л�״̬��
            //���Ҫ�л���״̬IDΪdefault,��ǰ״̬Ϊ��ʼ״̬
            //������״̬�б��ڲ�ѯ
            if (stateID == FSMStateID.Default)
                currentState = defaultState;
            else
                currentState = states.Find(s => s.StateID == stateID);
            //ִ�е�ǰ״̬�Ľ���
            currentState.OnEnterState(this);
        }
        #endregion

    }
}
