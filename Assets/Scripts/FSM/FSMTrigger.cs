using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI.FSM
{
    /// <summary>
    /// 所有条件的基类，之后的所有条件继承此类
    /// </summary>
    public abstract class FSMTrigger
    {
        /// <summary>
        /// 条件的编号,必不可少----------------------------1
        /// </summary>
        public FSMTriggerID TriggerID { get; set; }

        public FSMTrigger()
        {
            Init();
        }

        /// <summary>
        /// 要求子类必须初始化条件，为编号赋值--------------3
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// 逻辑处理（判断结果）----------------------------2
        /// 一般在判断是会用到一些变量
        /// 这些变量可以写在状态机里，这里我们用这种
        /// 也可以写在一个特定的类里  public abstract bool HandleTrigger(******* ******);
        /// </summary>
        /// <returns></returns>
        public abstract bool HandleTrigger(FSMBase fsmBase);
    }
}