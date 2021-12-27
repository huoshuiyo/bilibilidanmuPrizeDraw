using SQLite4Unity3d;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Data;

using UnityEngine;

public class SqliteController
{

    private static SqliteController sqliteController;

    private SQLiteConnection sqliteConnection;

    public string dataBaseFileName = "WinnersRecord.db";

    static public SqliteController Instance
    {
        get
        {
            if (sqliteController == null)
            {
                sqliteController = new SqliteController();
            }
            return sqliteController;
        }
    }

    //public SqliteController()
    //{
    //    InitSqliteController();
    //}

    public void Test()
    {
        //测试数据
        //Test test = new Test()
        //{
        //    id = "001",
        //    username = "hello"
        //};
        //CreateDataTable<Test>();
        //DeleteTabelAllData<Test>();
        //InsertData(test);
        //Debug.Log("Count:" + GetDataCount<Test>());
        //Debug.Log("Data:" + SelectData<Test>(t => t.id == "001"));
        //test.username = "hi";
        //InserDataIfNotExists(test, t => t.id == test.id, true);
        //Debug.Log("Count:" + GetDataCount<Test>());
        //Debug.Log("Data:" + SelectData<Test>(t => t.id == "001"));
    }

    #region 初始化
    public void OpenSqlite()
    {
        Debug.Log("dataBaseFileName:" + dataBaseFileName);
#if UNITY_EDITOR
        //Editor下可以直接使用streamingAssetsPath路径作为数据存储路径
        string dbPath = string.Format(Application.streamingAssetsPath + "/{0}", dataBaseFileName);
#else
            // 检查数据文件是否在 Application.persistentDataPath
            string filepath = string.Format ("{0}/{1}", Application.persistentDataPath, dataBaseFileName);
    
            if (!File.Exists (filepath)) {
                Debug.Log ("Database not in Persistent path");
                // 如果数据文件不在沙盒文件中需要先从StreamingAssets复制 ->

#if UNITY_ANDROID
                WWW loadDb =
     new WWW ("jar:file://" + Application.dataPath + "!/assets/" + dataBaseFileName); // Android下的StreamingAssets路径
                while (!loadDb.isDone) { } 
                //循环防止拷贝没完成
                File.WriteAllBytes (filepath, loadDb.bytes);
#elif UNITY_IOS
                string loadDb =
     Application.dataPath + "/Raw/" + dataBaseFileName; // iOS下的StreamingAssets路径
                //  iOS下拷贝
                File.Copy (loadDb, filepath);
#else
     //           string loadDb =
     //Application.dataPath + "/StreamingAssets/" + dataBaseFileName; // this is the path to your StreamingAssets in iOS
     //           // then save to Application.persistentDataPath
     //           File.Copy (loadDb, filepath);
#endif

			Debug.Log ("Database written");
		}

		var dbPath = filepath;
#endif
        sqliteConnection = new SQLiteConnection(dbPath);
        Debug.Log("Final PATH: " + dbPath);
    }

    #endregion

    #region 数据库操作

    public void UpdateWinnerExcluded()
    {
        if (sqliteConnection != null)
        {
            SQLiteCommand sQLiteCommand = sqliteConnection.CreateCommand("Update winnerinfo Set IsExcluded = 0");
            sQLiteCommand.ExecuteNonQuery();
        }
    }

    public void InsertWinner(string Uid, string Prize)
    {
        if (sqliteConnection != null)
        {
            string sql = string.Format("INSERT INTO WinnerInfo Values(\"{0}\",\"{1}\",{2})", Uid, Prize, 1);
            SQLiteCommand sQLiteCommand = sqliteConnection.CreateCommand(sql);
            sQLiteCommand.ExecuteNonQuery();
        }
    }

    public void ClearDB()
    {
        if (sqliteConnection != null)
        {
            string sql = "delete from WinnerInfo";
            SQLiteCommand sQLiteCommand = sqliteConnection.CreateCommand(sql);
            sQLiteCommand.ExecuteNonQuery();
        }
    }


    /// <summary>
    /// 删除数据库文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void DropTable<T>()
    {
        sqliteConnection.DropTable<T>();
    }

    /// <summary>
    /// 创建表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="isDrop"></param>
    public void CreateDataTable<T>(bool isDrop = false)
    {
        if (isDrop)
        {
            DropTable<T>();
        }
        sqliteConnection.CreateTable<T>();
    }



    /// <summary>
    /// 如果不存在则插入一条数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t">数据对象</param>
    /// <param name="predExpr">查询条件</param>
    /// <param name="isUpdate">如果存在则是否更新</param>
    public void InserDataIfNotExists<T>(T t, Expression<Func<T, bool>> predExpr, bool isUpdate = false) where T : new()
    {
        T tt = SelectData<T>(predExpr);
        if (tt == null)
        {
            sqliteConnection.Insert(t);
        }
        else if (isUpdate)
        {
            UpdateData(t);
        }
    }

    /// <summary>
    /// 插入一条数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    public void InsertData<T>(T t)
    {
        sqliteConnection.Insert(t);
    }

    /// <summary>
    /// 插入多条数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ts"></param>
    public void InsertDatas<T>(List<T> list)
    {
        sqliteConnection.InsertAll(list);
    }

    /// <summary>
    /// 获取数据条数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public int GetDataCount<T>() where T : new()
    {
        return sqliteConnection.Table<T>().Count();
    }

    /// <summary>
    /// 获取数据条数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="predExpr"></param>
    /// <returns></returns>
    public int GetDataCount<T>(Expression<Func<T, bool>> predExpr) where T : new()
    {
        return sqliteConnection.Table<T>().Count(predExpr);
    }

    /// <summary>
    /// 获取数据集
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IEnumerable<T> SelectDatas<T>() where T : new()
    {
        return sqliteConnection.Table<T>();
    }

    /// <summary>
    /// 获取数据集
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="predExpr"></param>
    /// <returns></returns>
    public IEnumerable<T> SelectDatas<T>(Expression<Func<T, bool>> predExpr) where T : new()
    {
        return sqliteConnection.Table<T>().Where(predExpr);
    }

    /// <summary>
    /// 获取单个数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="predExpr"></param>
    /// <returns></returns>
    public T SelectData<T>(Expression<Func<T, bool>> predExpr) where T : new()
    {
        return sqliteConnection.Table<T>().Where(predExpr).FirstOrDefault();
    }

    /// <summary>
    /// 修改数据并返回修改的行数,要有主键
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public int UpdateData<T>(T t)
    {
        return sqliteConnection.Update(t);
    }

    /// <summary>
    /// 修改数据并返回修改的行数,要有主键
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public int UpdateData<T>(List<T> list)
    {
        return sqliteConnection.Update(list);
    }




    /// <summary>
    /// 删除所有数据并返回删除行数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public int DeleteTabelAllData<T>()
    {
        
        return sqliteConnection.DeleteAll<T>();
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public int DeleteData<T>(T t)
    {
        return sqliteConnection.Delete(t);
    }

    /// <summary>
    /// 释放数据库资源
    /// </summary>
    public void Release()
    {
        sqliteConnection.Dispose();
    }
    #endregion
}
