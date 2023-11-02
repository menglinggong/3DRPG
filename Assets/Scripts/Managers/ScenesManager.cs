using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景管理器
/// </summary>
public class ScenesManager : ISingleton<ScenesManager>
{
    private GameObject player;
    private NavMeshAgent playerAgent;

    /// <summary>
    /// 传送
    /// </summary>
    /// <param name="transitionPoint"></param>
    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        if(transitionPoint.Type_Transition == TransitionPoint.TransitionType.SameScene)
        {
            //同场景传送
            StartCoroutine(Transition(null, transitionPoint.Type_Destination));
        }
        else
        {
            //异场景传送
            StartCoroutine(Transition(transitionPoint.SceneName, transitionPoint.Type_Destination));
        }
    }

    /// <summary>
    /// 加载场景并传送玩家
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    IEnumerator Transition(string sceneName, TransitionDestination.DestinationType destinationType)
    {
        if(player == null)
        {
            player = GameManager.Instance.PlayerStats.gameObject;
            playerAgent = player.GetComponent<NavMeshAgent>();
        }
        playerAgent.enabled = false;

        if (string.IsNullOrEmpty(sceneName))
        {
            var endPoint = PortalManager.Instance.GetTransitionDestinationByType(destinationType);
            //player.transform.position = endPoint.transform.position;
            //player.transform.rotation = endPoint.transform.rotation;
            player.transform.SetPositionAndRotation(endPoint.transform.position, endPoint.transform.rotation);
            playerAgent.enabled = true;
            yield return null;
        }
        else
        {
            var da = SceneManager.LoadSceneAsync(sceneName);
            //添加进度条
            while (da.isDone)
            {

                yield return null;
            }
        }
    }
}
