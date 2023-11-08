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
    /// <summary>
    /// 加载场景时进度条界面
    /// </summary>
    public LoadSceneProgressUI LoadSceneView;
    /// <summary>
    /// 玩家预制体
    /// </summary>
    public GameObject PlayerPrefab;
    /// <summary>
    /// 玩家对象
    /// </summary>
    private GameObject player;
    /// <summary>
    /// 玩家的导航网格
    /// </summary>
    private NavMeshAgent playerAgent;

    protected override void Awake()
    {
        base.Awake();
        LoadScene("Menu");
    }

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
            //异场景传送，加载场景，创建玩家到指定位置
            //TODO:添加加载场景进度条
            yield return StartCoroutine(LoadSceneBySceneName(sceneName));
            //yield return SceneManager.LoadSceneAsync(sceneName);
            //AsyncOperation async;
            //async.progress
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
        float progress = 0;
        LoadSceneView.OnProgressStart.Invoke();
        if (!SceneManager.GetActiveScene().name.Equals(sceneName))
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = true;

            //实际加载进度
            //while (operation.progress < 0.9f)
            //{
            //    Debug.Log(operation.progress);
            //    LoadSceneViewe.OnProgressChanged.Invoke(operation.progress);
            //}

            //不使用实际加载进度，太快看不见
            yield return new WaitForSeconds(1);
            
            while (progress < 1)
            {
                if(operation.progress < 0.9f)
                    progress += 0.1f;
                else
                    progress += 0.05f;

                LoadSceneView.OnProgressChanged.Invoke(progress);
            }
            //让进度条维持在100持续1秒
            yield return new WaitForSeconds(1);
            LoadSceneView.OnProgressDone.Invoke();
            yield return null;
        }

        //if (!SceneManager.GetActiveScene().name.Equals(sceneName))
        //{
        //    yield return SceneManager.LoadSceneAsync(sceneName);
        //}

    }
}
