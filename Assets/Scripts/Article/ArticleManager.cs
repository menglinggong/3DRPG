using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

/// <summary>
/// ��Ʒ������
/// </summary>
public class ArticleManager : ISingleton<ArticleManager>
{
    /// <summary>
    /// ӵ�е�������Ʒ��Ϣ
    /// </summary>
    [HideInInspector]
    public List<ArticleInfoBase> ArticleInfos = new List<ArticleInfoBase>();

    /// <summary>
    /// ��ǰѡ�е���Ʒ
    /// </summary>
    [HideInInspector]
    public ArticleInfoBase CurrentArticle = null;

    public float force = 300f;

    public float range = 3;

    /// <summary>
    /// ������
    /// </summary>
    private void Update()
    {
        
    }

    #region ���ݿ���ɾ�Ĳ鹦��

    /// <summary>
    /// �����Ʒ--�ӵ����ݿ���
    /// ���������ݿ��Ƿ����и���Ʒ��ֱ�����
    /// </summary>
    /// <param name="article"></param>
    private void AddArticle(ArticleInfoBase articleInfo)
    {
        //���浽���ݿ���
        SQLManager.Instance.OpenSQLaAndConnect();

        var fields = articleInfo.GetType().GetFields();
        string[] columns = new string[fields.Length];
        string[] columnValues = new string[fields.Length];
        for (int i = 0; i < fields.Length; i++)
        {
            columns[i] = fields[i].Name;
            columnValues[i] = fields[i].GetValue(articleInfo).ToString();
        }

        SQLManager.Instance.Insert(GetTableNameByID(articleInfo.ID), columns, columnValues);

        SQLManager.Instance.CloseSQLConnection();
    }

    /// <summary>
    /// �����Ʒ--�ӵ����ݿ���
    /// ��Ҫ������ݿ��Ƿ����и���Ʒ���������޸��������������
    /// </summary>
    /// <param name="articleInfo"></param>
    /// <param name="placeholder"></param>
    private void AddArticle(ArticleInfoBase articleInfo, bool placeholder)
    {
        string tableName = GetTableNameByID(articleInfo.ID);
        SQLManager.Instance.OpenSQLaAndConnect();

        var data = SQLManager.Instance.Select(tableName, null, new string[] { $"ID = {articleInfo.ID}" });

        if (!data.Read())
        {
            var fields = articleInfo.GetType().GetFields();
            string[] columns = new string[fields.Length];
            string[] columnValues = new string[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                columns[i] = fields[i].Name;
                columnValues[i] = fields[i].GetValue(articleInfo).ToString();
            }

            SQLManager.Instance.Insert(tableName, columns, columnValues);
        }
        else
        {
            var fields = articleInfo.GetType().GetFields();

            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].Name == "Count")
                {
                    int count = int.Parse(fields[i].GetValue(articleInfo).ToString());

                    int sqlIndex = data.GetOrdinal("Count");
                    int allCount = int.Parse(data.GetValue(sqlIndex).ToString());

                    SQLManager.Instance.Updata(tableName, new string[] { "Count" }, new string[] { (allCount + count).ToString() }, new string[] { $"ID = {articleInfo.ID}" });
                    break;
                }
            }
        }

        SQLManager.Instance.CloseSQLConnection();
    }

    /// <summary>
    /// �����ݿ��Ƴ���Ʒ
    /// ֱ���Ƴ�������Ҫ����Ʒ������
    /// </summary>
    /// <param name="articleInfo"></param>
    private void RemoveArticle(ArticleInfoBase articleInfo)
    {
        SQLManager.Instance.OpenSQLaAndConnect();

        SQLManager.Instance.Delete(GetTableNameByID(articleInfo.ID), new string[] { $"ID = {articleInfo.ID}" });

        SQLManager.Instance.CloseSQLConnection();
    }

    /// <summary>
    /// �����ݿ��Ƴ�
    /// �����Ƴ������������Ƴ�
    /// </summary>
    /// <param name="articleInfo"></param>
    private void RemoveArticle(ArticleInfoBase articleInfo, bool placeholder)
    {
        string tableName = GetTableNameByID(articleInfo.ID);

        SQLManager.Instance.OpenSQLaAndConnect();

        var data = SQLManager.Instance.Select(tableName, null, new string[] { $"ID = {articleInfo.ID}" });

        if (data.Read())
        {
            var fields = articleInfo.GetType().GetFields();

            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].Name == "Count")
                {
                    int count = int.Parse(fields[i].GetValue(articleInfo).ToString());

                    int sqlIndex = data.GetOrdinal("Count");
                    int allCount = int.Parse(data.GetValue(sqlIndex).ToString());

                    if (count >= allCount)
                    {
                        SQLManager.Instance.Delete(GetTableNameByID(articleInfo.ID), new string[] { $"ID = {articleInfo.ID}" });
                    }
                    else
                    {
                        SQLManager.Instance.Updata(tableName, new string[] { "Count" }, new string[] { (allCount - count).ToString() }, new string[] { $"ID = {articleInfo.ID}" });
                    }

                    break;
                }
            }
        }

        SQLManager.Instance.CloseSQLConnection();
    }

    /// <summary>
    /// ͨ����������ȡ���ݿ����ݣ�������ɵģ�
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public List<ArticleInfoBase> SelectArticle(string tableName)
    {
        SQLManager.Instance.OpenSQLaAndConnect();

        var data = SQLManager.Instance.Select(tableName);
        var articleInfos = ArticleManager.Instance.AnalysisSQLData(data);

        SQLManager.Instance.CloseSQLConnection();

        return articleInfos;
    }

    #endregion

    #region �������ݿ����ݣ��õ�������Ʒ����Ϣ

    /// <summary>
    /// �������ݿ����ݣ�����id�õ���Ӧ����Ʒ��Ϣ
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private List<ArticleInfoBase> AnalysisSQLData(SqliteDataReader data)
    {
        List<ArticleInfoBase> articleInfos = new List<ArticleInfoBase>();
        if(!data.IsDBNull(0))
        {
            int id = int.Parse(data[0].ToString());
            int key = id / 10000;

            switch (key)
            {
                case 1:
                    articleInfos = AnalysisSQLData_Weapon(data);
                    break;
                case 2:
                    articleInfos = AnalysisSQLData_Bow(data);
                    break;
                case 3:
                    articleInfos = AnalysisSQLData_Arrow(data);
                    break;
                case 4:
                    articleInfos = AnalysisSQLData_Shield(data);
                    break;
                case 5:
                    articleInfos = AnalysisSQLData_Cloth(data);
                    break;
                case 6:
                    articleInfos = AnalysisSQLData_SourceMaterial(data);
                    break;
                case 7:
                    //articleInfos = AnalysisSQLData_Weapon(data);
                    break;
                case 8:
                    //articleInfos = AnalysisSQLData_Weapon(data);
                    break;
            }
        }

        return articleInfos;
    }

    /// <summary>
    /// �������ݿ����ݣ��õ���������
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private List<ArticleInfoBase> AnalysisSQLData_Weapon(SqliteDataReader data)
    {
        List<ArticleInfoBase> weaponInfos = new List<ArticleInfoBase>();
        while (data.Read())
        {
            ArticleInfo_Weapon info = new ArticleInfo_Weapon();

            info.ID = int.Parse(data[0].ToString());
            info.Name = data[1].ToString();
            info.Descrip = data[2].ToString();
            info.IconPath = data[3].ToString();
            info.PrefabPath = data[4].ToString();
            info.Aggressivity = int.Parse(data[5].ToString());
            info.Durability = int.Parse(data[6].ToString());
            info.Enchant = (ArticleEnchanting)Enum.Parse(typeof(ArticleEnchanting), data[7].ToString());
            info.EnchantValue = int.Parse(data[8].ToString());
            info.WeaponKind = (WeaponKind)Enum.Parse(typeof(WeaponKind), data[9].ToString());
            info.HandKind = (WeaponKind_Hand)Enum.Parse(typeof(WeaponKind_Hand), data[10].ToString());
            info.MaterialKind = (ArticleKind_Material)Enum.Parse(typeof(ArticleKind_Material), data[11].ToString());
            info.EffectKind = (WeaponKind_Effect)Enum.Parse(typeof(WeaponKind_Effect), data[12].ToString());

            weaponInfos.Add(info);
        }

        return weaponInfos;
    }

    /// <summary>
    /// �������ݿ����ݣ��õ�������
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private List<ArticleInfoBase> AnalysisSQLData_Bow(SqliteDataReader data)
    {
        List<ArticleInfoBase> bowInfos = new List<ArticleInfoBase>();
        while (data.Read())
        {
            ArticleInfo_Bow info = new ArticleInfo_Bow();

            info.ID = int.Parse(data[0].ToString());
            info.Name = data[1].ToString();
            info.Descrip = data[2].ToString();
            info.IconPath = data[3].ToString();
            info.PrefabPath = data[4].ToString();
            info.Aggressivity = int.Parse(data[5].ToString());
            info.Durability = int.Parse(data[6].ToString());
            info.BowKind = (BowKind)Enum.Parse(typeof(BowKind), data[7].ToString());
            info.MaterialKind = (ArticleKind_Material)Enum.Parse(typeof(ArticleKind_Material), data[8].ToString());
            info.Range = int.Parse(data[9].ToString());
            info.Enchant = (ArticleEnchanting)Enum.Parse(typeof(ArticleEnchanting), data[10].ToString());
            info.EnchantValue = int.Parse(data[11].ToString());
            
            bowInfos.Add(info);
        }

        return bowInfos;
    }

    /// <summary>
    /// �������ݿ����ݣ��õ�������
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private List<ArticleInfoBase> AnalysisSQLData_Arrow(SqliteDataReader data)
    {
        List<ArticleInfoBase> arrowInfos = new List<ArticleInfoBase>();
        while (data.Read())
        {
            ArticleInfo_Arrow info = new ArticleInfo_Arrow();

            info.ID = int.Parse(data[0].ToString());
            info.Name = data[1].ToString();
            info.Descrip = data[2].ToString();
            info.IconPath = data[3].ToString();
            info.PrefabPath = data[4].ToString();
            info.ArrowKind = (ArrowKind)Enum.Parse(typeof(ArrowKind), data[5].ToString());
            info.Count = int.Parse(data[6].ToString());

            arrowInfos.Add(info);
        }

        return arrowInfos;
    }

    /// <summary>
    /// �������ݿ����ݣ��õ���������
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private List<ArticleInfoBase> AnalysisSQLData_Shield(SqliteDataReader data)
    {
        List<ArticleInfoBase> shieldInfos = new List<ArticleInfoBase>();
        while (data.Read())
        {
            ArticleInfo_Shield info = new ArticleInfo_Shield();

            info.ID = int.Parse(data[0].ToString());
            info.Name = data[1].ToString();
            info.Descrip = data[2].ToString();
            info.IconPath = data[3].ToString();
            info.PrefabPath = data[4].ToString();
            info.Defense = int.Parse(data[5].ToString());
            info.Durability = int.Parse(data[6].ToString());
            info.Enchant = (ArticleEnchanting)Enum.Parse(typeof(ArticleEnchanting), data[7].ToString());
            info.EnchantValue = int.Parse(data[8].ToString());
            info.MaterialKind = (ArticleKind_Material)Enum.Parse(typeof(ArticleKind_Material), data[9].ToString());

            shieldInfos.Add(info);
        }

        return shieldInfos;
    }

    /// <summary>
    /// �������ݿ����ݣ��õ���������
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private List<ArticleInfoBase> AnalysisSQLData_Cloth(SqliteDataReader data)
    {
        List<ArticleInfoBase> clothInfos = new List<ArticleInfoBase>();
        while (data.Read())
        {
            ArticleInfo_Cloth info = new ArticleInfo_Cloth();

            info.ID = int.Parse(data[0].ToString());
            info.Name = data[1].ToString();
            info.Descrip = data[2].ToString();
            info.IconPath = data[3].ToString();
            info.PrefabPath = data[4].ToString();
            info.ClothKind = (ClothKind)Enum.Parse(typeof(ClothKind), data[5].ToString());
            info.ClothLevel = (ClothLevel)Enum.Parse(typeof(ClothLevel), data[6].ToString());
            info.Defense = int.Parse(data[7].ToString());
            info.ClothEffect = (ClothEffect)Enum.Parse(typeof(ClothEffect), data[8].ToString());
            info.ClothEffectLevel = (ClothEffectLevel)Enum.Parse(typeof(ClothEffectLevel), data[9].ToString());

            clothInfos.Add(info);
        }

        return clothInfos;
    }

    /// <summary>
    /// �������ݿ����ݣ��õ��ز�����
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private List<ArticleInfoBase> AnalysisSQLData_SourceMaterial(SqliteDataReader data)
    {
        List<ArticleInfoBase> clothInfos = new List<ArticleInfoBase>();
        while (data.Read())
        {
            ArticleInfo_SourceMaterial info = new ArticleInfo_SourceMaterial();

            info.ID = int.Parse(data[0].ToString());
            info.Name = data[1].ToString();
            info.Descrip = data[2].ToString();
            info.IconPath = data[3].ToString();
            info.PrefabPath = data[4].ToString();
            info.Count = int.Parse(data[5].ToString());
            info.HealthRecoverValue = float.Parse(data[6].ToString());
            info.Effect = (SourceMaterialEffect)Enum.Parse(typeof(SourceMaterialEffect), data[7].ToString());
            info.EffectValue = float.Parse(data[8].ToString());
            info.EffectDuration = float.Parse(data[9].ToString());

            clothInfos.Add(info);
        }

        return clothInfos;
    }

    #endregion

    #region �ڲ�����

    /// <summary>
    /// ͨ����Ʒ��id�õ���Ʒ��ô�����Ǹ����ݿ����
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private string GetTableNameByID(int id)
    {
        int key = id / 10000;

        string tableName = "";

        switch (key)
        {
            case 1:
                tableName = SQLTableName.Article_Weapon;
                break;
            case 2:
                tableName = SQLTableName.Article_Bow;
                break;
            case 3:
                tableName = SQLTableName.Article_Arrow;
                break;
            case 4:
                tableName = SQLTableName.Article_Shield;
                break;
            case 5:
                tableName = SQLTableName.Article_Cloth;
                break;
            case 6:
                tableName = SQLTableName.Article_SourceMaterial;
                break;
            case 7:
                tableName = SQLTableName.Article_EndProduct;
                break;
            case 8:
                tableName = SQLTableName.Article_Import;
                break;
        }

        return tableName;
    }

    #endregion

    #region �ⲿ����

    /// <summary>
    /// ʰȡ��Ʒ
    /// </summary>
    /// <param name="articleInfo">��Ʒ��Ϣ</param>
    /// <param name="article">��Ʒ��3άģ��</param>
    /// <param name="isWithCount">��Ʒ�Ƿ����������Ϣ����������زģ���Щ�Ǵ���������Ϣ��ͬһid��Ʒ�����ظ���ӣ�</param>
    public void PickUpArticle(ArticleInfoBase articleInfo, Article article, bool isWithCount = false)
    {
        if (isWithCount)
            AddArticle(articleInfo, isWithCount);
        else
            AddArticle(articleInfo);

        ObjectPool.Instance.ReleaseObject(article.gameObject.name, article.gameObject);
    }

    /// <summary>
    /// ������Ʒ
    /// </summary>
    /// <param name="article"></param>
    public void FallDownArticle(Article article, bool isAddForce = true)
    {
        //1.����Ʒ�Ƶ�ȫ��λ��
        article.transform.SetParent(this.transform, true);
        //2.��Ʒ�ĳ�����Ϊ��
        article.Owner = null;
        //3.����Ʒһ������������һ����������Ч��
        if (isAddForce)
        {
            Rigidbody rd = article.GetComponent<Rigidbody>();
            rd.AddExplosionForce(force, article.transform.position - Vector3.up, range);
            float x = UnityEngine.Random.Range(-1, 1);
            float z = UnityEngine.Random.Range(-1, 1);
            rd.AddExplosionForce(force * 0.5f, article.transform.position + new Vector3(x, 0, z), range);
        }
    }

    /// <summary>
    /// ������Ʒ
    /// </summary>
    /// <param name="articlePath">��ƷԤ�����·��</param>
    /// <param name="pos">��Ʒ���ɵ�λ��</param>
    public void FallDownArticle(string articlePath, Vector3 pos)
    {
        string[] paths = articlePath.Split('/');
        string name = paths[paths.Length - 1];
        GameObject article = ObjectPool.Instance.GetObject(name, articlePath);
        article.transform.position = pos;
        FallDownArticle(article.GetComponent<Article>());
    }

    /// <summary>
    /// ʹ����Ʒ��
    /// </summary>
    /// <param name="articleInfo"></param>
    public void UseArticle(ArticleInfoBase articleInfo)
    {

    }

    /// <summary>
    /// �ֳ���Ʒ
    /// </summary>
    /// <param name="articleInfo"></param>
    public void HoldArticle(ArticleInfoBase articleInfo)
    {

    }

    /// <summary>
    /// ������Ʒ
    /// </summary>
    /// <param name="articleInfo"></param>
    public void DropArticle(ArticleInfoBase articleInfo)
    {

    }

    /// <summary>
    /// װ����Ʒ
    /// </summary>
    /// <param name="articleInfo"></param>
    public void EquipArticle(ArticleInfoBase articleInfo)
    {
        string[] paths = articleInfo.PrefabPath.Split('/');
        string name = paths[paths.Length -1];

        //1.������Ʒ��ʵ��
        GameObject article = ObjectPool.Instance.GetObject(name, articleInfo.PrefabPath);
        article.GetComponent<Article>().Owner = GameManager.Instance.PlayerStats.transform;
        article.GetComponent<Rigidbody>().useGravity = false;
        article.SetActive(true);

        //2.������ҵ�װ����Ʒ�ķ���
        GameManager.Instance.PlayerStats.GetComponent<PlayerController>().EquipArticle(articleInfo, article);
    }



    #endregion



}
