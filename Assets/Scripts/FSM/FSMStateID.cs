using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI.FSM
{
    /// <summary>
    /// 状态的枚举，存储所有的状态，赋予状态ID
    /// 后续如果状态改变或新增可修改
    /// </summary>
    public enum FSMStateID
    {
        Default,
        //站桩/守卫（不会自己巡逻）
        Guard,
        //巡逻
        Patrol,
        //追击
        Chase,
        //攻击
        Attack,
        //胜利
        Win,
        //死亡
        Dead
    }
}