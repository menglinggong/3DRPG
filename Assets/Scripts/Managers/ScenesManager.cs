using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景管理器
/// </summary>
public class ScenesManager : ISingleton<ScenesManager>
{
    public GameObject PlayerPrefab;
    private GameObject player;
    private NavMeshAgent playerAgent;

    /// <summary>
    /// 传送
    /// </summary>
    /// <param name="transitionPoint"></param>
    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        if (transitionPoint.Type_Destination == TransitionDestination.DestinationType.Not)
            return;

        if(transitionPoint.Type_Transition == TransitionPoint.TransitionType.SameScene)
        {
            //同场景传送
            StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.Type_Destination));
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
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            if (player == null)
            {
                player = GameManager.Instance.PlayerStats.gameObject;
                playerAgent = player.GetComponent<NavMeshAgent>();
            }
            playerAgent.enabled = false;

            //同场景传送，设置玩家位置
            var endPoint = PortalManager.Instance.GetTransitionDestinationByType(destinationType);
            player.transform.SetPositionAndRotation(endPoint.transform.position, endPoint.transform.rotation);
            playerAgent.enabled = true;
            yield return null;
        }
        else
        {
            ////TODO:保存玩家数据
            //SaveDataManager.Instance.SavePlayerData();
            //异场景传送，加载场景，创建玩家到指定位置
            //TODO:添加加载场景进度条
            yield return SceneManager.LoadSceneAsync(sceneName);
            var endPoint = PortalManager.Instance.GetTransitionDestinationByType(destinationType);
            yield return Instantiate(PlayerPrefab, endPoint.transform.position, endPoint.transform.rotation);

            yield break;
        }
    }

    /// <summary>
    /// 普通的加载场景
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneBySceneName(sceneName));
    }

    IEnumerator LoadSceneBySceneName(string sceneName)
    {
        if(!SceneManager.GetActiveScene().name.Equals(sceneName))
            yield return SceneManager.LoadSceneAsync(sceneName);
    }
}
