using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>
    /// 状态机
    /// </summary>
    public class FSMBase : MonoBehaviour
    {
        /// <summary>
        /// 敌人的控制器
        /// </summary>
        private EnemyController enemyController;

        public EnemyController EnemyController
        {
            get
            {
                return enemyController;
            }
        }

        #region 状态机自身数据
        /// <summary>
        /// 状态列表
        /// </summary>
        private List<FSMState> states;

        /// <summary>
        /// 当前状态
        /// </summary>
        private FSMState currentState;

        /// <summary>
        /// 默认状态
        /// </summary>
        private FSMState defaultState;

        [Tooltip("默认初始状态")]
        public FSMStateID defaultStateID;

        /// <summary>
        /// 文件名称（在streamingAssets下）
        /// </summary>
        public string FileName;

        private void Start()
        {
            ConfigFSM();
            enemyController = this.gameObject.GetComponent<EnemyController>();
            InitDefalutState();
        }

        /// <summary>
        /// 配置状态机
        /// </summary>
        private void ConfigFSM()
        {
            states = new List<FSMState>();
            //1.创建状态对象（调用FSMState.AddMap）2.设置当前状态
            //#region 旧的方法，通过手写添加，当状态或者条件添加或修改需要修改此代码，不好
            //当有具体状态类后（如IdelState与DeadState）执行
            //创建状态对象
            ////IdelState idel = new IdelState();
            //调用FSMState.AddMap 添加状态与条件的映射（有几种映射添加几次）
            ////idel.AddMap(FSMTriggerID.NoHealth, FSMStateID.Dead);
            //idel.AddMap(FSMTriggerID.SawTarget)
            //将此状态加入到状态机的状态列表内
            ////states.Add(idel);

            //添加死亡状态逻辑与上面一样
            ////DeadState dead = new DeadState();
            //由于死亡后没有其他状态所以没有映射不用AddMap
            ////states.Add(dead);
            //有其他状态继续添加
            //#endregion
            #region 新的方法，通过读取配置文件添加，当状态或者条件添加或修改时只需修改配置文件即可，代码不用修改
            //当多个NPC使用一样的配置文件时，此时会重复解析相同文件，会增加消耗，所以可以用一个新方法
            //var map = new AIConfigurationReader(fileName).map;
            //新方法，使用工厂存储解析过的ai文件
            //StringBuilder path = new StringBuilder(Application.streamingAssetsPath);
            //path.Append("/");
            //path.Append(FilePath);
            
            var map = AIConfigurationReaderFactory.GetMap(FileName);
            //Application.streamingAssetsPath
            foreach (var stateName in map)
            {
                //stateName.key -->当前状态
                //stateName.value--->映射
                Type type = Type.GetType("AI.FSM." + stateName.Key + "State");
                FSMState state = Activator.CreateInstance(type) as FSMState;

                foreach (var values in stateName.Value)
                {
                    //values.key -->条件编号
                    //values.value --->状态编号
                    //string 转 enum
                    FSMTriggerID triggerID = (FSMTriggerID)Enum.Parse(typeof(FSMTriggerID), values.Key);
                    FSMStateID stateID = (FSMStateID)Enum.Parse(typeof(FSMStateID), values.Value);
                    //添加映射
                    state.AddMap(triggerID, stateID);
                }
                //添加状态
                states.Add(state);
            }
            #endregion
        }

        /// <summary>
        /// 初始化当前状态
        /// </summary>
        private void InitDefalutState()
        {
            defaultState = states.Find(s => s.StateID == defaultStateID);
            currentState = defaultState;
            //执行初始状态的进入
            currentState.OnEnterState(this);
        }

        /// <summary>
        /// 每帧处理的逻辑
        /// </summary>
        public void Update()
        {
            if (currentState == null) return;

            //判断当前状态的条件
            currentState.Reason(this);
            //执行当前状态的逻辑
            currentState.OnStayState(this);
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="stateID"></param>
        public void ChangeState(FSMStateID stateID)
        {
            //执行上一个状态的退出
            currentState.OnExitState(this);

            //设置当前状态（切换状态）
            //如果要切换的状态ID为default,则当前状态为初始状态
            //否则在状态列表内查询
            if (stateID == FSMStateID.Default)
                currentState = defaultState;
            else
                currentState = states.Find(s => s.StateID == stateID);
            //执行当前状态的进入
            currentState.OnEnterState(this);
        }
        #endregion

    }
}
