

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserProfileModel{
    public int id;
    public string name;
    public int level;
    public DateTime created_at;
    public DateTime updated_ad;
}


public class UserProfile
{
    /* �?ーブル生�?? */
    public static void CreateUserProfileTable(){
        string query = "create table if not exists user_profile (id integer, name text,level integer, primary key (id))";
        //SQLiteのファイルを取�?
        SqliteDatabase sqlDB = new SqliteDatabase(GameUtil.Const.SQLITE_FILE_NAME);
        //SQLiteのファイルにTableを生�?
        sqlDB.ExecuteQuery(query);
    }

    //レコード登録
    public static void Set(UserProfileModel user_profile){
Debug.Log(user_profile.id+"AAA");
Debug.Log(user_profile.name+"BBB");
Debug.Log(user_profile.level+"CCC");

        string query = "insert into user_profile values ('" + user_profile.id + "','" + user_profile.name + "','" + user_profile.level +"')";
        SqliteDatabase sqlDB = new SqliteDatabase(GameUtil.Const.SQLITE_FILE_NAME);
        sqlDB.ExecuteNonQuery(query);
        Debug.Log("Insert完了");
    }

    //レコード取�?
    public static UserProfileModel Get(){
        string query = "select * from user_profile";
        SqliteDatabase sqlDB = new SqliteDatabase(GameUtil.Const.SQLITE_FILE_NAME);
        //user_profile�?ーブルからレコードを全件取�?
        DataTable dataTable = sqlDB.ExecuteQuery(query);
        UserProfileModel userProfileModel = new UserProfileModel();
        foreach (DataRow dr in dataTable.Rows){
            userProfileModel.id = (int)dr["id"];
            userProfileModel.name = dr["name"].ToString();
            userProfileModel.level = (int)dr["level"];
        }
        return userProfileModel;
    }

    public static void Delete(){
        string query = "delete from user_profile";
        SqliteDatabase sqlDB = new SqliteDatabase(GameUtil.Const.SQLITE_FILE_NAME);
        sqlDB.ExecuteQuery(query);
    }
}

