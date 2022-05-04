

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserProfileModel{
    public string user_id;
    public string user_name;
    public DateTime created_at;
    public DateTime updated_ad;
}


public class UserProfile
{
    public string user_id;
    public string user_name;

    /* �?ーブル生�?? */
    public static void CreateUserProfileTable(){
        string query = "create table if not exists user_profile (user_id text, user_name text, primary key (user_id))";
        //SQLiteのファイルを取�?
        SqliteDatabase sqlDB = new SqliteDatabase(GameUtil.Const.SQLITE_FILE_NAME);
        //SQLiteのファイルにTableを生�?
        sqlDB.ExecuteQuery(query);
    }

    //レコード登録
    public static void Set(UserProfileModel user_profile){
        string query = "insert into user_profile values ('" + user_profile.user_id + "','" + user_profile.user_name + "')";
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
            userProfileModel.user_id = dr["user_id"].ToString();
            userProfileModel.user_name = dr["user_name"].ToString();
        }
        return userProfileModel;
    }

    public static void Delete(){
        string query = "delete from user_profile";
        SqliteDatabase sqlDB = new SqliteDatabase(GameUtil.Const.SQLITE_FILE_NAME);
        sqlDB.ExecuteQuery(query);
    }
}

