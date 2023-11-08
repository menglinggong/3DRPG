using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

/// <summary>
/// ����������
/// </summary>
public class ScenesManager : ISingleton<ScenesManager>
{
    /// <summary>
    /// ���س���ʱ����������
    /// </summary>
    public LoadSceneProgressUI LoadSceneView;
    /// <summary>
    /// ���Ԥ����
    /// </summary>
    public GameObject PlayerPrefab;
    /// <summary>
    /// ��Ҷ���
    /// </summary>
    private GameObject player;
    /// <summary>
    /// ��ҵĵ�������
    /// </summary>
    private NavMeshAgent playerAgent;

    protected override void Awake()
    {
        base.Awake();
        LoadScene("Menu");
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="transitionPoint"></param>
    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        if (transitionPoint.Type_Destination == TransitionDestination.DestinationType.Not)
            return;

        if(transitionPoint.Type_Transition == TransitionPoint.TransitionType.SameScene)
        {
            //ͬ��������
            StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.Type_Destination));
        }
        else
        {
            //�쳡������
            StartCoroutine(Transition(transitionPoint.SceneName, transitionPoint.Type_Destination));
        }
    }

    /// <summary>
    /// ���س������������
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

            //ͬ�������ͣ��������λ��
            var endPoint = PortalManager.Instance.GetTransitionDestinationByType(destinationType);
            player.transform.SetPositionAndRotation(endPoint.transform.position, endPoint.transform.rotation);
            playerAgent.enabled = true;
            yield return null;
        }
        else
        {
            //�쳡�����ͣ����س�����������ҵ�ָ��λ��
            //TODO:��Ӽ��س���������
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
    /// ��ͨ�ļ��س���
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

            //ʵ�ʼ��ؽ���
            //while (operation.progress < 0.9f)
            //{
            //    Debug.Log(operation.progress);
            //    LoadSceneViewe.OnProgressChanged.Invoke(operation.progress);
            //}

            //��ʹ��ʵ�ʼ��ؽ��ȣ�̫�쿴����
            yield return new WaitForSeconds(1);
            
            while (progress < 1)
            {
                if(operation.progress < 0.9f)
                    progress += 0.1f;
                else
                    progress += 0.05f;

                LoadSceneView.OnProgressChanged.Invoke(progress);
            }
            //�ý�����ά����100����1��
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
