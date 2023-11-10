using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 石头人控制器
/// </summary>
public class Golem : EnemyController
{
    /// <summary>
    /// 石头人手的部位
    /// </summary>
    public Transform Hand;

    /// <summary>
    /// 石头示例
    /// </summary>
    public GameObject RockPrefab;

    /// <summary>
    /// 创建出的石头
    /// </summary>
    private GameObject rock;

    /// <summary>
    /// Animation event
    /// 创建石头
    /// </summary>
    public void CreateRock()
    {
        rock = ObjectPool.Instance.GetObject(RockPrefab.name, RockPrefab);

        if(rock == null )
        {
            rock = Instantiate(RockPrefab, Hand.position, Quaternion.identity);
            rock.name = RockPrefab.name;
        }
        rock.transform.localScale = Vector3.one;
        rock.SetActive(true);
        rock.GetComponent<Rigidbody>().isKinematic = true;
        rock.transform.SetParent(Hand, true);
        rock.transform.localPosition = Vector3.zero;
        rock.transform.localRotation = Quaternion.identity;
        //
    }

    /// <summary>
    /// Animation event
    /// 投掷石头
    /// </summary>
    public void HurlRock()
    {
        Rock go = rock.GetComponent<Rock>();
        rock.transform.SetParent(null);
        go.Target = this.attackTarget;
        rock.GetComponent<Rigidbody>().velocity = Vector3.one;
        rock.GetComponent<Rigidbody>().isKinematic = false;

        //TODO:石头的伤害值是固定值
        //go.RockDamage = 10;
        go.FlyToTarget();
    }

}
