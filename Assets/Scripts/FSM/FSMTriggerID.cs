using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI.FSM
{
    /// <summary>
    /// 条件的枚举，存储所有的条件，赋予条件ID
    /// 后续如果条件改变或新增可修改
    /// </summary>
    public enum FSMTriggerID
    {
        //生命值为0
        NoHealth,
        //发现目标
        SawTarget,
        //目标进入攻击范围
        ReachTarget,
        //丢失目标
        LoseTarget,
        //完成巡逻
        CompletePatrol,
        //击杀目标
        KilledTarget,
        //目标离开攻击范围
        WithoutAttackRange,
        //.......
    }
}