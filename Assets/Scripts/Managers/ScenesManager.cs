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
    public GameObject PlayerPrefab;
    private GameObject player;
    private NavMeshAgent playerAgent;

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
            ////TODO:�����������
            //SaveDataManager.Instance.SavePlayerData();
            //�쳡�����ͣ����س�����������ҵ�ָ��λ��
            //TODO:��Ӽ��س���������
            yield return SceneManager.LoadSceneAsync(sceneName);
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
        if(!SceneManager.GetActiveScene().name.Equals(sceneName))
            yield return SceneManager.LoadSceneAsync(sceneName);
    }
}
