using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.ParticleSystem;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using System.Linq;

/// <summary>
/// 物品管理器
/// </summary>
public class ArticleManager : ISingleton<ArticleManager>
{
    /// <summary>
    /// 拥有的所有物品信息
    /// </summary>
    [HideInInspector]
    public List<ArticlesData> articles = new List<ArticlesData>();

    /// <summary>
    /// 当前选中的物品
    /// </summary>
    [HideInInspector]
    public ArticleInfoBase CurrentArticle = null;

    /// <summary>
    /// 当前选中的物品格子
    /// </summary>
    [HideInInspector]
    public ItemFrame CurrentItemFram = null;

    public float force = 300f;

    public float range = 3;

    /// <summary>
    /// 测试用
    /// </summary>
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightAlt))
        {
            SerializeArticles();
        }
    }

    #region 数据库增删改查功能---目前不使用，使用本地json文件保存物品数据

    /// <summary>
    /// 添加物品--加到数据库中
    /// 无需检查数据库是否已有该物品，直接添加
    /// </summary>
    /// <param name="article"></param>
    private void AddArticle(ArticleInfoBase articleInfo)
    {
        //保存到数据库中
        SQLManager.Instance.OpenSQLAndConnect();

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
    /// 添加物品--加到数据库中
    /// 需要检查数据库是否已有该物品，若有则修改数量，否则添加
    /// </summary>
    /// <param name="articleInfo"></param>
    /// <param name="placeholder"></param>
    private void AddArticle(ArticleInfoBase articleInfo, bool placeholder)
    {
        string tableName = GetTableNameByID(articleInfo.ID);
        SQLManager.Instance.OpenSQLAndConnect();

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
    /// 从数据库移除物品
    /// 直接移除，不需要管物品的数量
    /// </summary>
    /// <param name="articleInfo"></param>
    private void RemoveArticle(ArticleInfoBase articleInfo)
    {
        SQLManager.Instance.OpenSQLAndConnect();

        //TODO需要修改，不然相同id的武器不同属性会删错
        SQLManager.Instance.Delete(GetTableNameByID(articleInfo.ID), new string[] { $"ID = {articleInfo.ID}" });

        SQLManager.Instance.CloseSQLConnection();
    }

    /// <summary>
    /// 从数据库移除
    /// 根据移除的数量进行移除
    /// </summary>
    /// <param name="articleInfo"></param>
    private void RemoveArticle(ArticleInfoBase articleInfo, bool placeholder)
    {
        string tableName = GetTableNameByID(articleInfo.ID);

        SQLManager.Instance.OpenSQLAndConnect();

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
    /// 通过表名，获取数据库数据（解析完成的）
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public List<ArticleInfoBase> SelectArticle(string tableName)
    {
        SQLManager.Instance.OpenSQLAndConnect();

        var data = SQLManager.Instance.Select(tableName);
        var articleInfos = ArticleManager.Instance.AnalysisSQLData(data);

        SQLManager.Instance.CloseSQLConnection();

        return articleInfos;
    }

    #endregion


    #region 本地文件形式的物品数据

    /// <summary>
    /// 添加物品
    /// </summary>
    /// <param name="articleInfo"></param>
    private void AddArticle_Local(ArticleInfoBase articleInfo)
    {
        Type type = articleInfo.GetType();
        var data = articles.Find(a => a.Type == type.Name);

        if (data == null)
        {
            ArticlesData articlesData = new ArticlesData();
            articlesData.Type = type.Name;
            articlesData.ArticleInfos = new List<ArticleInfoBase>();
            articlesData.ArticleInfos.Add(articleInfo);
            articles.Add(articlesData);
        }
        else
        {
            data.ArticleInfos.Add(articleInfo);
        }
    }

    /// <summary>
    /// 添加物品--带有数量数据的
    /// </summary>
    /// <param name="articleInfo"></param>
    /// <param name="isWithCount"></param>
    private void AddArticle_Local(ArticleInfoBase articleInfo, bool isWithCount)
    {
        Type type = articleInfo.GetType();
        var data = articles.Find(a => a.Type == type.Name);

        if (data == null)
        {
            ArticlesData articlesData = new ArticlesData();
            articlesData.Type = type.Name;
            articlesData.ArticleInfos = new List<ArticleInfoBase>();
            articlesData.ArticleInfos.Add(articleInfo);
            articles.Add(articlesData);
        }
        else
        {
            var info = data.ArticleInfos.Find(a=>a.ID ==  articleInfo.ID);
            if (info == null)
                data.ArticleInfos.Add(articleInfo);
            else
            {
                var fields = info.GetType().GetFields();

                foreach (var item in fields)
                {
                    if(item.Name == "Count")
                    {
                        int count = int.Parse(item.GetValue(info).ToString());
                        int addCount = int.Parse(item.GetValue(articleInfo).ToString());

                        item.SetValue(info, count + addCount);
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 移除物品
    /// </summary>
    /// <param name="articleInfo"></param>
    private void RemoveArticle_Local(ArticleInfoBase articleInfo)
    {
        Type type = articleInfo.GetType();
        var data = articles.Find(a => a.Type == type.Name);

        if (data != null)
        {
            var info = data.ArticleInfos.Find(a => a.ID == articleInfo.ID);
            if(info != null)
                data.ArticleInfos.Remove(info);
            else
                Debug.LogError("出错，不存在该物品数据！");
        }
        else
        {
            Debug.LogError("出错，不存在该物品数据！");
        }
    }

    /// <summary>
    /// 移除物品--按数量移除
    /// </summary>
    /// <param name="articleInfo"></param>
    /// <param name="isWithCount"></param>
    private void RemoveArticle_Local(ArticleInfoBase articleInfo, bool isWithCount)
    {
        Type type = articleInfo.GetType();
        var data = articles.Find(a => a.Type == type.Name);

        if (data != null)
        {
            var info = data.ArticleInfos.Find(a => a.ID == articleInfo.ID);
            if (info != null)
            {
                var fields = info.GetType().GetFields();

                foreach (var item in fields)
                {
                    if (item.Name == "Count")
                    {
                        int count = int.Parse(item.GetValue(info).ToString());
                        int removeCount = int.Parse(item.GetValue(articleInfo).ToString());

                        int value = count - removeCount;
                        if(value < 0)
                        {
                            Debug.LogError("出错，物品数量不够！");
                        }
                        else if(value == 0)
                        {
                            data.ArticleInfos.Remove(info);
                            item.SetValue(info, value);
                        }
                        else
                            item.SetValue(info, value);

                        break;
                    }
                }
            }
            else
                Debug.LogError("出错，不存在该物品数据！");
        }
        else
        {
            Debug.LogError("出错，不存在该物品数据！");
        }
    }

    /// <summary>
    /// 获取指定类型的物品数据
    /// </summary>
    /// <param name="type"></param>
    public List<ArticleInfoBase> GetArticles(string type)
    {
        var data = articles.Find(a => a.Type == type);
        if(data == null)
            return null;
        else
            return data.ArticleInfos;
    }

    #endregion


    #region 解析数据库数据，得到各种物品的信息---目前不使用

    /// <summary>
    /// 解析数据库数据，根据id得到对应的物品信息
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
    /// 解析数据库数据，得到武器数据
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
    /// 解析数据库数据，得到弓数据
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
    /// 解析数据库数据，得到箭数据
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
    /// 解析数据库数据，得到盾牌数据
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
    /// 解析数据库数据，得到服饰数据
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
    /// 解析数据库数据，得到素材数据
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

    #region 内部方法

    /// <summary>
    /// 通过物品的id得到物品因该存放在那个数据库表内
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

    #region 外部方法

    /// <summary>
    /// 拾取物品
    /// </summary>
    /// <param name="articleInfo">物品信息</param>
    /// <param name="article">物品的3维模型</param>
    /// <param name="isWithCount">物品是否带有数量信息（例如箭和素材，这些是带有数量信息，同一id物品不能重复添加）</param>
    public void PickUpArticle(ArticleInfoBase articleInfo, Article article, bool isWithCount = false)
    {
        if (isWithCount)
            AddArticle_Local(articleInfo, isWithCount);
        else
            AddArticle_Local(articleInfo);

        ObjectPool.Instance.ReleaseObject(article.gameObject.name, article.gameObject);
    }

    /// <summary>
    /// 掉落物品
    /// </summary>
    /// <param name="article"></param>
    public void FallDownArticle(Article article, bool isAddForce = true)
    {
        //1.把物品移到全局位置
        article.transform.SetParent(this.transform, true);
        //2.物品的持有者为空
        article.Owner = null;
        //3.给物品一个力，让它有一个飞起掉落的效果
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
    /// 掉落物品
    /// </summary>
    /// <param name="articlePath">物品预制体的路径</param>
    /// <param name="pos">物品生成的位置</param>
    public void FallDownArticle(string articlePath, Vector3 pos)
    {
        string[] paths = articlePath.Split('/');
        string name = paths[paths.Length - 1];
        GameObject article = ObjectPool.Instance.GetObject(name, articlePath);
        article.transform.position = pos;
        FallDownArticle(article.GetComponent<Article>());
    }

    /// <summary>
    /// 丢弃物品
    /// </summary>
    /// <param name="articleInfo"></param>
    public void DropArticle(ArticleInfoBase articleInfo)
    {

    }

    /// <summary>
    /// 装备物品
    /// </summary>
    /// <param name="articleInfo"></param>
    public void EquipArticle(ArticleInfoBase articleInfo)
    {
        string[] paths = articleInfo.PrefabPath.Split('/');
        string name = paths[paths.Length -1];

        //1.创建物品的实例
        GameObject article = ObjectPool.Instance.GetObject(name, articleInfo.PrefabPath);
        article.GetComponent<Article>().Owner = GameManager.Instance.PlayerStats.transform;
        article.GetComponent<Rigidbody>().useGravity = false;
        article.SetActive(true);

        //2.调用玩家的装备物品的方法
        GameManager.Instance.PlayerStats.GetComponent<PlayerController>().EquipArticle(articleInfo, article);
    }

    /// <summary>
    /// 卸下物品
    /// </summary>
    /// <param name="articleInfo"></param>
    public void DisboardArticle(ArticleInfoBase articleInfo)
    {

    }

    /// <summary>
    /// 丢弃物品
    /// </summary>
    /// <param name="articleInfo"></param>
    public void DiscardArticle(List<ArticleInfoBase> articleInfos)
    {

    }

    /// <summary>
    /// 使用物品
    /// </summary>
    /// <param name="articleInfo"></param>
    public void UseArticle(ArticleInfoBase articleInfo)
    {
        Type type = articleInfo.GetType();

        var fields = type.GetFields();

        foreach (var field in fields)
        {
            if (field.Name == "Count")
            {
                RemoveArticle_Local(articleInfo, true);
                return;
            }
        }

        RemoveArticle_Local(articleInfo);
    }

    /// <summary>
    /// 手持物品
    /// </summary>
    /// <param name="articleInfo"></param>
    public void HoldArticle(List<ArticleInfoBase> articleInfos)
    {

    }

    /// <summary>
    /// 查看物品
    /// </summary>
    /// <param name="articleInfo"></param>
    public void CheckArticle(ArticleInfoBase articleInfo)
    {

    }

    /// <summary>
    /// 序列化物品数据到本地json文件
    /// </summary>
    public void SerializeArticles()
    {
        string filePath = Application.streamingAssetsPath + "/Articles/Articles.Json";

        if (!File.Exists(filePath))
        {
            File.Create(filePath);
        }
        string value = JsonConvert.SerializeObject(articles);

        Debug.Log("序列化成功");
        File.WriteAllText(filePath, value);
    }

    /// <summary>
    /// 反序列化物品数据
    /// </summary>
    public void DeserializeArticles()
    {
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new ArticleInfoConverter());

        string filePath = Application.streamingAssetsPath + "/Articles/Articles.Json";

        if (File.Exists(filePath))
        {
            string data = File.ReadAllText(filePath);
            Debug.Log("反序列化成功");
            articles = JsonConvert.DeserializeObject<List<ArticlesData>>(data, settings);
        }
    }

    #endregion


}

/// <summary>
/// 存储不同物品类型及其数据列表
/// </summary>
[Serializable]
public class ArticlesData
{
    public string Type;
    public List<ArticleInfoBase> ArticleInfos;
}

/// <summary>
/// 物品信息的序列化与反序列化转化器
/// </summary>
public class ArticleInfoConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(List<ArticlesData>);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        List<ArticlesData> datas = new List<ArticlesData>();

        if (reader.TokenType == JsonToken.StartArray)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndArray)
                    break;

                if (reader.TokenType == JsonToken.StartObject)
                {
                    ArticlesData data = new ArticlesData();
                    JObject obj = JObject.Load(reader);
                    data.Type = obj["Type"].ToString();
                    Type type = Type.GetType(data.Type);

                    data.ArticleInfos = new List<ArticleInfoBase>();

                    JToken[] infos = obj["ArticleInfos"].ToArray();
                    
                    foreach (var info in infos)
                    {
                        ArticleInfoBase articleInfo = info.ToObject(type) as ArticleInfoBase;
                        data.ArticleInfos.Add(articleInfo);
                    }

                    datas.Add(data);
                }
            }
        }

        return datas;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
