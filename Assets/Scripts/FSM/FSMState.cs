using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI.FSM
{
    /// <summary>
    /// 所有状态的基类，之后的所有状态继承此类
    /// </summary>
    public abstract class FSMState
    {
        /// <summary>
        /// 状态的编号，必不可少--------------------------------------------------------1
        /// </summary>
        public FSMStateID StateID { get; set; }

        /// <summary>
        /// 条件与状态的映射表--------------------------------------------------------------3
        /// </summary>
        private Dictionary<FSMTriggerID, FSMStateID> map;

        /// <summary>
        /// 条件集-------------------------------------------------------------------------4
        /// </summary>
        private List<FSMTrigger> Triggers;

        public FSMState()
        {
            map = new Dictionary<FSMTriggerID, FSMStateID>();

            Triggers = new List<FSMTrigger>();

            Init();
        }

        /// <summary>
        /// 判断当前状态的哪个条件满足，切换至下一个状态
        /// </summary>
        public void Reason(FSMBase fsmBase)
        {
            foreach (var trigger in Triggers)
            {
                if (trigger.HandleTrigger(fsmBase))
                {
                    //条件满足，切换状态
                    FSMStateID stateID = map[trigger.TriggerID];
                    fsmBase.ChangeState(stateID);
                    return;
                }
            }
        }

        /// <summary>
        /// 要求子类必须初始化状态，为编号赋值------------------------------------------------2
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// 由状态机调用，添加映射表与条件集数据----------------------------------------------5
        /// </summary>
        /// <param name="triggerID"></param>
        /// <param name="stateID"></param>
        public void AddMap(FSMTriggerID triggerID, FSMStateID stateID)
        {
            //添加映射
            map.Add(triggerID, stateID);
            //创建条件对象添加至条件集
            AddTrigger(triggerID);
        }

        /// <summary>
        /// 创建条件对象添加至条件集-----------------------------------------------------------6
        /// </summary>
        /// <param name="triggerID"></param>
        private void AddTrigger(FSMTriggerID triggerID)
        {
            //使用反射，创建条件对象
            //反射的命名规范：AI.FSM + triggerID + Trigger
            Type type = Type.GetType("AI.FSM." + triggerID + "Trigger");
            FSMTrigger trigger = Activator.CreateInstance(type) as FSMTrigger;
            Triggers.Add(trigger);
        }

        //为子类提供可选实现------------------------------------------------------------------------7

        // 一般在状态里会用到一些变量
        // 这些变量可以写在状态机里，这里我们用这种
        // 也可以写在一个特定的类里  public virtual void OnEnterState(******* ******);
        public virtual void OnEnterState(FSMBase fsmBase) { }

        public virtual void OnStayState(FSMBase fsmBase) { }

        public virtual void OnExitState(FSMBase fsmBase) { }
    }
}