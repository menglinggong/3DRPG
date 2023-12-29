using RPG.Skill;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// ��ҿ�����
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region ���

    /// <summary>
    /// ����
    /// </summary>
    private NavMeshAgent agent;

    /// <summary>
    /// ����������
    /// </summary>
    private Animator animator;

    /// <summary>
    /// ��ҵ���ֵ
    /// </summary>
    private CharacterStats characterStats;

    /// <summary>
    /// ���ƣ�����״̬����ʾ
    /// </summary>
    private GameObject defenceShield;

    /// <summary>
    /// ��ҵļ���ϵͳ
    /// </summary>
    private CharacterSkillSystem characterSkillSystem;

    #endregion

    #region ��ҵĻ�������

    /// <summary>
    /// ����
    /// </summary>
    private GameObject attackTarget;

    /// <summary>
    /// ������Э��
    /// </summary>
    private Coroutine AttackCoroutine;

    /// <summary>
    /// ��һ�ι�����ʱ��
    /// </summary>
    private float lastAttackTime;

    /// <summary>
    /// �Ƿ�����
    /// </summary>
    private bool isDie = false;

    /// <summary>
    /// ��ҳ�ʼֹͣ����
    /// </summary>
    private float stopDistance;

    #endregion

    #region ����

    public CharacterStats CharacterStats
    {
        get { return characterStats; }
    }

    #endregion

    private Coroutine turnRoundCoroutine = null;

    /// <summary>
    /// ���ˮƽ����ĽǶ�
    /// </summary>
    private float cameraXValue;

    #region �������ں���

    private void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        characterStats = this.GetComponent<CharacterStats>();
        characterSkillSystem = this.GetComponent<CharacterSkillSystem>();

        defenceShield = this.transform.Find("Defence").gameObject;
        GameManager.Instance.RegisterPlayer(characterStats);
    }

    private void Start()
    {
        agent.speed = characterStats.CharacterData.MoveSpeed;
        stopDistance = agent.stoppingDistance;
    }

    private void OnEnable()
    {
        SaveDataManager.Instance.LoadPlayerData();

        EventManager.Instance.AddListener(MessageConst.InputSystemConst.OnAPerformed, OnAPerformed);
        EventManager.Instance.AddListener(MessageConst.InputSystemConst.OnLeftStick, OnLeftStickInput);
        EventManager.Instance.AddListener(MessageConst.OnCameraMove, OnCameraMove);
        
        //��Ҽ������ݺ�����Ϣ������Ѫ���;�����
        EventManager.Instance.Invoke(MessageConst.UpdateHealth, characterStats);
        EventManager.Instance.Invoke(MessageConst.UpdateExp, characterStats);
    }

    private void OnDisable()
    {
        SaveDataManager.Instance.SavePlayerData();
        EventManager.Instance.RemoveListener(MessageConst.InputSystemConst.OnAPerformed, OnAPerformed);
        EventManager.Instance.RemoveListener(MessageConst.InputSystemConst.OnLeftStick, OnLeftStickInput);
        EventManager.Instance.RemoveListener(MessageConst.OnCameraMove, OnCameraMove);
    }

    private void Update()
    {
        if (isDie || GameManager.Instance.IsGamePaused) return;

        SearchNearEstArticle();

        SwitchAnimation(); 

        //���㹥�����ʱ��
        if(lastAttackTime > 0)
            lastAttackTime -= Time.deltaTime;

        if(characterStats.CharacterData.CurrentHealth <= 0 && !isDie)
        {
            isDie = true;
            agent.enabled = false;
            animator.SetTrigger("Die");
            GameManager.Instance.NotifyObserver();
        }

        //TODO���԰�Q�ż���
        if (Input.GetKeyDown(KeyCode.Q))
        {
            characterSkillSystem.AttackUseSkill(1, false);
        }

        Defence();
    }

    #endregion

    #region �ڲ�����

    /// <summary>
    /// ����Ƿ����
    /// </summary>
    private void Defence()
    {
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("Dizzy") || animator.GetCurrentAnimatorStateInfo(1).IsName("GetHit"))
            return;

        if (Input.GetMouseButton(1))
        {
            characterStats.IsDefence = true;
            agent.isStopped = true;
            defenceShield.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            characterStats.IsDefence = false;
            defenceShield.SetActive(false);
        }
    }

    /// <summary>
    /// ������Ҷ����л�
    /// </summary>
    private void SwitchAnimation()
    {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }

    /// <summary>
    /// ����Ŀ��
    /// </summary>
    /// <param name="target"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void EventAttack(GameObject target)
    {
        if (target != null)
        {
            this.attackTarget = target;

            //ֹͣ�ƶ������˵�Э��
            if (AttackCoroutine != null)
                StopCoroutine(AttackCoroutine);

            AttackCoroutine = StartCoroutine(MoveToAttackTarget());
        }
    }

    /// <summary>
    /// �ƶ���Ŀ��
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveToAttackTarget()
    {
        //ʹ��ҿ��ƶ�
        agent.isStopped = false;
        //��������ƶ�����������
        agent.stoppingDistance = characterStats.CharacterData.AttackRange;
        //����˾�����ڹ������룬�ƶ�
        while (Vector3.Distance(this.transform.position, attackTarget.transform.position) > characterStats.CharacterData.AttackRange)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        //���빥����Χ�����й���
        //1.ʹ��Ҳ����ƶ�
        agent.isStopped = true;
        //2.���㹥�����ʱ��
        if (lastAttackTime <= 0)
        {
            Attack();
        }
    }

    /// <summary>
    /// ��ͨ����
    /// </summary>
    private void Attack()
    {
        //ת�����
        if (turnRoundCoroutine != null)
            StopCoroutine(turnRoundCoroutine);

        turnRoundCoroutine = StartCoroutine(transform.TurnRound(attackTarget.transform.position, characterStats.CharacterData.TurnRoundSpeed));

        //��ͨ�������㼼�ܣ���ʹ�ü���ϵͳ���߼�
        lastAttackTime = 1f / characterStats.CharacterData.AttackSpeed;

        characterStats.CalculateCritical(characterStats);
        //���������򲥷ű������������򲥷���ͨ��������
        animator.SetBool("CriticalAttack", characterStats.IsCritical);
        animator.SetTrigger("Attack");
    }

    #endregion


    #region �ⲿ����

    /// <summary>
    /// ����˺����ڲ��Ŷ���ʱ����
    /// </summary>
    public void Hit()
    {
        //TODO:�����¼�ʹ�ü���ϵͳ�ķ���
        if (attackTarget == null) return;

        if (attackTarget.CompareTag("AttackAble"))
        {
            Rock rock = attackTarget.GetComponent<Rock>();
            if (rock != null)
            {
                rock.GetComponent<Rigidbody>().velocity = Vector3.one;
                rock.rockState = RockStates.HitEnemy;
                //ʯͷ���˺�ֵ�ǹ̶�ֵ
                //rock.RockDamage = 10;

                //��ʯͷʩ��һ�������ǰ�Ϸ�����
                rock.GetComponent<Rigidbody>().AddForce((this.transform.forward + Vector3.up * 0.5f).normalized * rock.HitPlayerForce, ForceMode.Impulse);
            }
        }
        else
        {
            CharacterStats targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    #endregion


    #region ��Ʒװ�����

    private Transform weaponPoint;
    private Transform shieldPoint;

    /// <summary>
    /// �洢�����ڿ�ʰȡ��Χ�ڵ���Ʒ
    /// </summary>
    private Dictionary<int, Transform> articles = new Dictionary<int, Transform>();

    /// <summary>
    /// ��ʰȡ����Ʒ
    /// </summary>
    private Transform pickUpArticle = null;

    /// <summary>
    /// ͨ����Ʒid�õ���Ʒװ��λ��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private Transform GetArticlePoint(int id)
    {
        int key = id / 10000;
        Transform articlePoint = null;
        switch (key)
        {
            case 1:             //����
                if (weaponPoint == null)
                    weaponPoint = this.transform.Find("WeaponPoint");
                articlePoint = weaponPoint;
                break;
            //case 2:
            //    break;
            case 4:             //����
                if (shieldPoint == null)
                    shieldPoint = this.transform.Find("ShieldPoint");
                articlePoint = shieldPoint;
                break;
        }

        return articlePoint;
    }

    /// <summary>
    /// ���봥����
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //�ж��Ƿ�Ϊ��ʰȡ��Ʒ
        if (other.CompareTag("Article") && other.gameObject.activeSelf)
        {
            int key = other.gameObject.GetInstanceID();

            if(!articles.ContainsKey(key))
            {
                articles.Add(key, other.transform);
            }
        }
    }

    /// <summary>
    /// �뿪������
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Article") && other.gameObject.activeSelf)
        {
            int key = other.gameObject.GetInstanceID();
            if (articles.ContainsKey(key))
            {
                articles.Remove(key);
            }
        }
    }

    /// <summary>
    /// �жϾ����������Ʒ����ʾ��ʰȡ���
    /// </summary>
    private void SearchNearEstArticle()
    {
        float minDistance = 5;
        Transform nearestArticle = null;
        foreach (var article in articles)
        {
            float dis = Vector3.Distance(this.transform.position, article.Value.position);
            if(dis < minDistance)
            {
                minDistance = dis;
                nearestArticle = article.Value;
            }
        }

        if(nearestArticle != null)
        {
            pickUpArticle = nearestArticle;
            EventManager.Instance.Invoke(MessageConst.ArticleConst.OnShowHideArticleInfo, nearestArticle.GetComponent<Article>());
        }
        else
        {
            pickUpArticle = null;
            EventManager.Instance.Invoke(MessageConst.ArticleConst.OnShowHideArticleInfo, null);
        }
    }

    /// <summary>
    /// װ����Ʒ
    /// </summary>
    /// <param name="articleInfo"></param>
    /// <param name="article"></param>
    public void EquipArticle(ArticleInfoBase articleInfo, GameObject article)
    {
        //1.�ж�Ҫװ������Ʒ�����ͣ���Ҫװ����ʲôλ��
        Transform articlePoint = GetArticlePoint(articleInfo.ID);

        //2.��ԭʼ����Ʒ�������أ��滻���µ���Ʒ
        if(articlePoint.childCount != 0)
        {
            GameObject go = articlePoint.GetChild(0).gameObject;
            ObjectPool.Instance.ReleaseObject(go.name, go);
        }
        article.transform.SetParent(articlePoint, false);

        //3.������Ʒ�����Զ���ҵ����Խ��е���
        //TODO��
    }

    #endregion


    #region ���̺��ֱ������ļ���

    /// <summary>
    /// ����A�����Ĭ�ϼ��̵�U������ʰȡ��Ʒ
    /// </summary>
    /// <param name="messageConst"></param>
    /// <param name="data"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnAPerformed(string messageConst, object data)
    {
        if (GameManager.Instance.IsGamePaused) return;

        if (pickUpArticle != null)
        {
            pickUpArticle.GetComponent<Article>().PickUp();

            int key = pickUpArticle.gameObject.GetInstanceID();
            if (articles.ContainsKey(key))
            {
                articles.Remove(key);
            }
            pickUpArticle = null;

            EventManager.Instance.Invoke(MessageConst.ArticleConst.OnShowHideArticleInfo, null);
        }
    }

    /// <summary>
    /// �ֱ���ҡ��/�������������
    /// </summary>
    /// <param name="messageConst"></param>
    /// <param name="data"></param>
    private void OnLeftStickInput(string messageConst, object data)
    {
        if (GameManager.Instance.IsGamePaused) return;

        Vector2 value = (Vector2)data;
        if (value == Vector2.zero) return;

        Vector3 moveValue = new Vector3(value.x, 0, value.y);
        Quaternion pianyi = Quaternion.Euler(0, cameraXValue, 0);
        moveValue = pianyi * moveValue;

        Vector3 targetPos = this.transform.position + (moveValue * (stopDistance + 0.2f));

        //ʹ��ҿ��ƶ�
        agent.isStopped = false;
        
        agent.stoppingDistance = stopDistance;
        agent.destination = targetPos;
    }

    /// <summary>
    /// ����ӽ�ת��
    /// </summary>
    /// <param name="messageConst"></param>
    /// <param name="data"></param>
    private void OnCameraMove(string messageConst, object data)
    {
        cameraXValue = (float)data;
    }


    #endregion



}
