using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    /// <summary>
    /// 人物数值对象
    /// </summary>
    public CharacterData_SO CharacterData;

    /// <summary>
    /// 最大生命值属性
    /// </summary>
    public float MaxHealth
    {
        get
        {
            return CharacterData != null ? CharacterData.MaxHealth : 0;
        }
        set
        {
            CharacterData.MaxHealth = value;
        }
    }

    /// <summary>
    /// 最大生命值属性
    /// </summary>
    public float CurrentHealth
    {
        get
        {
            return CharacterData != null ? CharacterData.CurrentHealth : 0;
        }
        set
        {
            CharacterData.CurrentHealth = value;
        }
    }

    public float BaseDefence
    {
        get
        {
            return CharacterData != null ? CharacterData.BaseDefence : 0;
        }
        set
        {
            CharacterData.BaseDefence = value;
        }
    }

    public float CurrentDefence
    {
        get
        {
            return CharacterData != null ? CharacterData.CurrentDefence : 0;
        }
        set
        {
            CharacterData.CurrentDefence = value;
        }
    }
}
