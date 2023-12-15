using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

/// <summary>
/// ����������
/// </summary>
public class InventoryManager : ISingleton<InventoryManager>
{
    /// <summary>
    /// ��ǰѡ�е���Ʒ
    /// </summary>
    [HideInInspector]
    public ArticleInfoBase CurrentArticle = null;
    
    /// <summary>
    /// �����Ʒ--�ӵ����ݿ���
    /// </summary>
    /// <param name="article"></param>
    public void AddInventoryArticle(ArticleInfoBase articleInfo)
    {
        //TODO,�ж������������Ʒ��ֱ����ӣ���������Ʒ�����ݿ����Ѵ��ڣ����޸ĳ�������
        //���浽���ݿ���
        SQLManager.Instance.OpenSQLaAndConnect();

        var fields = articleInfo.GetType().GetFields();
        string[] columns = new string[fields.Length];
        string[] columnValues = new string[fields.Length];
        for (int i = 0; i < fields.Length; i++)
        {
            columns[i] = fields[i].Name;
            columnValues[i] = fields[i].GetValue(articleInfo).ToString();
            //Debug.Log($"{columns[i]} = {columnValues[i]}");
        }

        SQLManager.Instance.Insert(GetTableNameByID(articleInfo.ID), columns, columnValues);

        SQLManager.Instance.CloseSQLConnection();

        
    }

    /// <summary>
    /// ͨ����Ʒ��id�õ���Ʒ��ô�����Ǹ����ݿ����
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetTableNameByID(int id)
    {
        int key = id / 10000;

        string tableName = "";

        switch(key)
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

    /// <summary>
    /// �������ݿ����ݣ�����id�õ���Ӧ����Ʒ��Ϣ
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public List<ArticleInfoBase> AnalysisSQLData(SqliteDataReader data)
    {
        List<ArticleInfoBase> articleInfos = new List<ArticleInfoBase>();
        if(data != null)
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
                    //articleInfos = AnalysisSQLData_Weapon(data);
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
            info.Aggressivity = float.Parse(data[5].ToString());
            info.Durability = float.Parse(data[6].ToString());
            info.Enchant = (ArticleEnchanting)Enum.Parse(typeof(ArticleEnchanting), data[7].ToString());
            info.EnchantValue = float.Parse(data[8].ToString());
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
            info.Aggressivity = float.Parse(data[5].ToString());
            info.Durability = float.Parse(data[6].ToString());
            info.BowKind = (BowKind)Enum.Parse(typeof(BowKind), data[7].ToString());
            info.MaterialKind = (ArticleKind_Material)Enum.Parse(typeof(ArticleKind_Material), data[8].ToString());
            info.Range = float.Parse(data[9].ToString());
            info.Enchant = (ArticleEnchanting)Enum.Parse(typeof(ArticleEnchanting), data[10].ToString());
            info.EnchantValue = float.Parse(data[11].ToString());
            
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
            info.Defense = float.Parse(data[5].ToString());
            info.Durability = float.Parse(data[6].ToString());
            info.Enchant = (ArticleEnchanting)Enum.Parse(typeof(ArticleEnchanting), data[7].ToString());
            info.EnchantValue = float.Parse(data[8].ToString());
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
            info.Defense = float.Parse(data[7].ToString());
            info.ClothEffect = (ClothEffect)Enum.Parse(typeof(ClothEffect), data[8].ToString());
            info.ClothEffectLevel = (ClothEffectLevel)Enum.Parse(typeof(ClothEffectLevel), data[9].ToString());

            clothInfos.Add(info);
        }

        return clothInfos;
    }


    ///// <summary>
    ///// ������Ʒ--ȫ��
    ///// </summary>
    ///// <param name="item"></param>
    //public void RemoveInventoryItem(InventoryItem item)
    //{
    //    if (inventoryItemDict.ContainsKey(item.Id))
    //    {
    //        inventoryItemDict.Remove(item.Id);
    //        currentInventoryItem = null;
    //    }
    //}

    ///// <summary>
    ///// ������Ʒ--����
    ///// </summary>
    ///// <param name="id"></param>
    //public void RemoveInventoryItem(int id)
    //{
    //    if (inventoryItemDict.ContainsKey(id))
    //    {
    //        inventoryItemDict[id].Count--;

    //        if (inventoryItemDict[id].Count <= 0)
    //        {
    //            inventoryItemDict.Remove(id);
    //            currentInventoryItem = null;
    //        }
    //    }
    //}

    ///// <summary>
    ///// ������Ʒ--����
    ///// </summary>
    ///// <param name="id"></param>
    ///// <param name="count"></param>
    //public void RemoveInventoryItem(int id, int count)
    //{
    //    if (inventoryItemDict.ContainsKey(id))
    //    {
    //        inventoryItemDict[id].Count -= count;

    //        if (inventoryItemDict[id].Count <= 0)
    //        {
    //            inventoryItemDict.Remove(id);
    //            currentInventoryItem = null;
    //        }
    //    }
    //}

    ///// <summary>
    ///// ʹ����Ʒ--ȫ������
    ///// </summary>
    ///// <param name="item"></param>
    //public void UseInventoryItem(InventoryItem item)
    //{
    //    //TODO:ʹ����Ʒ

    //    RemoveInventoryItem(item);
    //}

    ///// <summary>
    ///// ʹ����Ʒ--����ʹ��
    ///// </summary>
    ///// <param name="item"></param>
    //public void UseInventoryItem(int id)
    //{
    //    //TODO:ʹ����Ʒ

    //    RemoveInventoryItem(id);
    //}

    ///// <summary>
    ///// ʹ����Ʒ--����ʹ��
    ///// </summary>
    ///// <param name="item"></param>
    //public void UseInventoryItem(int id, int count)
    //{
    //    //TODO:ʹ����Ʒ

    //    RemoveInventoryItem(id, count);
    //}
}
