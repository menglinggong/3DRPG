using Mono.Data.Sqlite;
using System.Data.Common;
using System;
using System.Text;
using UnityEngine;


/// <summary>
/// 功能说明：SQLite数据库操作
/// 作者：孟令公
/// 日期：2022-3-18
/// </summary>
public class SQLManager
{
    /// <summary>
    /// 数据库连接对象
    /// </summary>
    private SqliteConnection connection;
    /// <summary>
    /// 数据库命令
    /// </summary>
    private SqliteCommand command;
    /// <summary>
    /// 数据读取定义
    /// </summary>
    private SqliteDataReader reader;

    /// <summary>
    /// 单例
    /// </summary>
    private static SQLManager _instance;
    /// <summary>
    /// 获取单例
    /// </summary>
    public static SQLManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new SQLManager();
            return _instance;
        }
    }

    /// <summary>
    /// 开启连接数据库
    /// </summary>
    public void OpenSQLaAndConnect()
    {
        this.connection = new SqliteConnection("data source=" + Application.streamingAssetsPath + "/SQLite/3DRPG.db");
        this.connection.Open();
    }
    /// <summary>
    /// 关闭数据库连接,注意这一步非常重要，最好每次测试结束的时候都调用关闭数据库连接
    /// 如果不执行这一步，多次调用之后，会报错，数据库被锁定，每次打开都非常缓慢
    /// </summary>
    public void CloseSQLConnection()
    {
        if (this.command != null)
        {
            this.command.Cancel();
        }

        if (this.reader != null)
        {
            this.reader.Close();
        }

        if (this.connection != null)
        {
            this.connection.Close();

        }
        this.command = null;
        this.reader = null;
        this.connection = null;
        Debug.Log("已经断开数据库连接");
    }

    /// <summary>
    /// 执行SQL命令,并返回一个SqliteDataReader对象
    /// </summary>
    /// <param name="queryString">SQL语句</param>
    /// <returns>表内数据</returns>
    private SqliteDataReader ExecuteSQLCommand(string queryString)
    {
        this.command = this.connection.CreateCommand();
        this.command.CommandText = queryString;
        this.reader = this.command.ExecuteReader();
        return this.reader;
    }

    /// <summary>
    /// 执行SQL命令，并返回影响数据的数量
    /// </summary>
    /// <param name="queryString">SQL语句</param>
    /// <returns>影响数据的数量</returns>
    private int ExecuteNonQuery(string queryString)
    {
        this.command = this.connection.CreateCommand();
        this.command.CommandText = queryString;
        //this.reader = this.command.ExecuteReader();
        return this.command.ExecuteNonQuery();
    }


    #region 数据库查询操作

    /// <summary>
    /// 数据库查询--整表查询
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public SqliteDataReader Select(string tableName)
    {
        string queryString = "SELECT * FROM " + tableName;

        return this.ExecuteSQLCommand(queryString);
    }

    /// <summary>
    /// 数据库查询
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <param name="columnNames">单个列</param>
    /// <param name="where">限制条件</param>
    /// <param name="orderBy">排序规则</param>
    /// <param name="limit">查找数量</param>
    /// <returns></returns>
    public SqliteDataReader Select(string tableName, string columnName, string where, string orderBy, int limit)
    {
        StringBuilder builder = new StringBuilder();
        //字符串组合
        builder.Append(string.Format("SELECT {0} FROM {1}", columnName, tableName));
        if (!string.IsNullOrEmpty(where)) builder.Append(" WHERE " + where);
        if (!string.IsNullOrEmpty(orderBy)) builder.Append(" ORDER BY " + orderBy);
        if (limit != -1) builder.Append(" LIMIT " + limit);
        builder.Append(';');

        string sql = builder.ToString();

        return this.ExecuteSQLCommand(sql);
    }

    /// <summary>
    /// 数据库查询
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="columnNames"></param>
    /// <param name="wheres"></param>
    /// <param name="orderBy"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public SqliteDataReader Select(string tableName, string[] columnNames = null, string[] wheres = null, string orderBy = null, int limit = -1)
    {
        char quote = '\'';
        char comma = ',';
        string and = " AND ";
        StringBuilder builder1 = new StringBuilder();
        StringBuilder builder2 = new StringBuilder();
        if (columnNames == null)
        {
            builder1.Append('*');
        }
        else
        {
            foreach (var column in columnNames)
            {
                builder1.Append(column).Append(comma);
            }
            builder1.Remove(builder1.Length - 1, 1);
        }

        string where = null;
        if (wheres != null)
        {
            foreach (var tmpWhere in wheres)
            {
                string[] strs = StringSplit(tmpWhere);
                builder2.Append(strs[0]).Append(strs[1]).Append(quote).Append(strs[2]).Append(quote).Append(and);
                //builder2.Append(tmpWhere).Append(and);
            }
            builder2.Remove(builder2.Length - and.Length, and.Length);
            where = builder2.ToString();
        }

        return Select(tableName, builder1.ToString(), where, orderBy, limit);
    }

    /// <summary>
    /// 数据库查询（多表联查）
    /// </summary>
    /// <param name="tableNames">表名</param>
    /// <param name="columnNames">列名</param>
    /// <param name="wheres">限制条件</param>
    /// <param name="orderBy">排序规则</param>
    /// <param name="limit">查询数量</param>
    /// <returns></returns>
    public DbDataReader Select(string[] tableNames, string[] columnNames, string[] wheres = null, string orderBy = null, int limit = -1)
    {
        char comma = ',';
        string and = " AND ";
        StringBuilder builder0 = new StringBuilder();
        StringBuilder builder1 = new StringBuilder();
        StringBuilder builder2 = new StringBuilder();
        foreach (var tableName in tableNames)
        {
            builder0.Append(tableName).Append(comma);
        }
        builder0.Remove(builder0.Length - 1, 1);
        foreach (var column in columnNames)
        {
            builder1.Append(column).Append(comma);
        }
        builder1.Remove(builder1.Length - 1, 1);
        string where = null;
        if (wheres != null)
        {
            foreach (var tmpWhere in wheres)
            {
                builder2.Append(tmpWhere).Append(and);
            }
            builder2.Remove(builder2.Length - and.Length, and.Length);
            where = builder2.ToString();
        }
        return Select(builder0.ToString(), builder1.ToString(), where, orderBy, limit);
    }

    #endregion

    #region 数据库增，删，改，清空操作

    /// <summary>
    /// 更新数据库内表的数据
    /// </summary>
    /// <param name="tableName">表名称</param>
    /// <param name="columnNames">表字段</param>
    /// <param name="columnValues">对应的值</param>
    /// <param name="wheres">条件</param>
    /// <returns>影响的数量</returns>
    public int Updata(string tableName, string[] columnNames, string[] columnValues, string[] wheres)
    {
        if (columnNames.Length != columnValues.Length)
        {
            UnityEngine.Debug.LogError("所选字段数目与提供的值数目不匹配，无法更新数据。");
            return -3;
        }

        char quote = '\'';
        char comma = ',';
        char equal = '=';
        string and = " AND ";
        StringBuilder builder1 = new StringBuilder();
        StringBuilder builder2 = new StringBuilder();

        for (int i = 0; i < columnNames.Length; i++)
        {
            //拼接表名与对应数据
            builder1.Append(columnNames[i]).Append(equal).Append(quote).Append(columnValues[i]).Append(quote).Append(comma);
        }
        builder1.Remove(builder1.Length - 1, 1);

        foreach (var where in wheres)
        {
            string[] strs = StringSplit(where);

            builder2.Append(strs[0]).Append(strs[1]).Append(quote).Append(strs[2]).Append(quote).Append(and);

            //builder2.Append(where).Append(and);
        }
        builder2.Remove(builder2.Length - and.Length, and.Length);

        string queryString = string.Format("UPDATE {0} SET {1} WHERE {2};", tableName, builder1.ToString(), builder2.ToString());

        return ExecuteNonQuery(queryString);
    }

    /// <summary>
    /// 在表中插入数据
    /// </summary>
    /// <param name="tableName">表名称</param>
    /// <param name="columnNames">表字段</param>
    /// <param name="columnValues">字段对应的值</param>
    /// <returns>插入的数量</returns>
    public int Insert(string tableName, string[] columnNames, string[] columnValues)
    {
        if (columnNames.Length != columnValues.Length)
        {
            UnityEngine.Debug.LogError("所选字段数目与提供的值数目不匹配，无法更新数据。");
            return -3;
        }

        char qc = '\'';
        char comma = ',';
        StringBuilder columnName = new StringBuilder();
        StringBuilder columnValue = new StringBuilder();

        foreach (var field in columnNames)
        {
            columnName.Append(field).Append(comma);
        }
        columnName.Remove(columnName.Length - 1, 1);

        foreach (var value in columnValues)
        {
            //固定表名结构
            columnValue.Append(qc).Append(value).Append(qc).Append(comma);
        }
        columnValue.Remove(columnValue.Length - 1, 1);

        //数据库语句
        string sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2});", tableName, columnName.ToString(), columnValue.ToString());
        //Debug.Log(sql);
        return ExecuteNonQuery(sql);

    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <param name="wheres">限制条件</param>
    /// <returns></returns>
    public int Delete(string tableName, string[] wheres)
    {
        //char eq = '\"';
        char quote = '\'';
        //char equal = '=';
        string and = " AND ";
        StringBuilder builder = new StringBuilder();
        foreach (var where in wheres)
        {
            string[] strs = StringSplit(where);
            builder.Append(strs[0]).Append(strs[1]).Append(quote).Append(strs[2]).Append(quote).Append(and);

            //builder.Append(where).Append(and);
        }
        builder.Remove(builder.Length - and.Length, and.Length);

        //数据库语句
        string sql = string.Format("DELETE FROM {0} WHERE {1};", tableName, builder.ToString());

        return ExecuteNonQuery(sql);
    }

    /// <summary>
    /// 清空数据库表数据
    /// </summary>
    /// <param name="tableName"></param>
    public SqliteDataReader Clear(string tableName)
    {
        string queryString = "DELETE FROM " + tableName;
        return this.ExecuteSQLCommand(queryString);
    }

    #endregion

    /// <summary>
    /// 标准化条件字段
    /// </summary>
    /// <param name="where">条件</param>
    /// <returns>标准格式</returns>
    public string[] StringSplit(string where)
    {
        string[] wheres = new string[3];

        char[] c = new char[] { '>', '=', '<', '!' };
        var strs = where.Split(c);

        wheres[0] = strs[0];
        wheres[2] = strs[strs.Length - 1];

        var chars = where.ToCharArray();

        string str = "";
        foreach (var item in chars)
        {
            switch (item)
            {
                case '>':
                    str += '>';
                    break;
                case '<':
                    str += '<';
                    break;
                case '!':
                    str += '!';
                    break;
                case '=':
                    str += '=';
                    break;
                default:
                    break;
            }
        }

        wheres[1] = str;

        return wheres;
    }

}

/// <summary>
/// 数据库表名
/// </summary>
public partial class SQLTableName
{
    /// <summary>
    /// 武器
    /// </summary>
    public const string Article_Weapon = "Article_Weapon";
    /// <summary>
    /// 弓
    /// </summary>
    public const string Article_Bow = "Article_Bow";
    /// <summary>
    /// 箭
    /// </summary>
    public const string Article_Arrow = "Article_Arrow";
    /// <summary>
    /// 盾牌
    /// </summary>
    public const string Article_Shield = "Article_Shield";
    /// <summary>
    /// 服饰
    /// </summary>
    public const string Article_Cloth = "Article_Cloth";
    /// <summary>
    /// 素材
    /// </summary>
    public const string Article_SourceMaterial = "Article_SourceMaterial";
    /// <summary>
    /// 成品
    /// </summary>
    public const string Article_EndProduct = "Article_EndProduct";
    /// <summary>
    /// 重要物品
    /// </summary>
    public const string Article_Import = "Article_Import";
}

