using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharProfileModel{
    public string char_id;
    public string char_name;
    public int char_attack;
    public int char_hp;
    public DateTime created_at;
    public DateTime updated_ad;
}


public class CharProfile
{
    public string char_id;
    public string char_name;

    /* �?ーブル生�?? */
    public static void CreateCharProfileTable(){
        string query = "create table if not exists char_profile (char_id text, char_name text, primary key (char_id))";
        //SQLiteのファイルを取�?
        SqliteDatabase sqlDB = new SqliteDatabase(GameUtil.Const.SQLITE_FILE_NAME);
        //SQLiteのファイルにTableを生�?
        sqlDB.ExecuteQuery(query);
    }

    //レコード登録
    public static void Set(CharProfileModel char_profile){
        string query = "insert into char_profile values ('" + char_profile.char_id + "','" + char_profile.char_name + "')";
        SqliteDatabase sqlDB = new SqliteDatabase(GameUtil.Const.SQLITE_FILE_NAME);
        sqlDB.ExecuteNonQuery(query);
        Debug.Log("Insert完了");
    }

    //レコード取�?
    public static List<CharProfileModel> Get(){
        string query = "select * from char_profile";
        SqliteDatabase sqlDB = new SqliteDatabase(GameUtil.Const.SQLITE_FILE_NAME);
        //char_profile�?ーブルからレコードを全件取�?
        DataTable dataTable = sqlDB.ExecuteQuery(query);
        List<CharProfileModel> charProfileModelArr = new List<CharProfileModel>();
        CharProfileModel charProfileModel = new CharProfileModel();
        foreach (DataRow dr in dataTable.Rows){
            charProfileModel.char_id = dr["char_id"].ToString();
            charProfileModel.char_name = dr["char_name"].ToString();
            charProfileModelArr.Add(charProfileModel);
        }
        return charProfileModelArr;
    }

    public static void Delete(){
        string query = "delete from char_profile";
        SqliteDatabase sqlDB = new SqliteDatabase(GameUtil.Const.SQLITE_FILE_NAME);
        sqlDB.ExecuteQuery(query);
    }
}

