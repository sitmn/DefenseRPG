using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class UserLogin : MonoBehaviour
{
    [SerializeField]
    private GameObject Canvas;
    [SerializeField]
    private GameObject SignInObj;
    [SerializeField]
    private GameObject SignOnObj;
    [SerializeField]
    private InputField input_name;
    [SerializeField]
    private Text login_text;
    [SerializeField]
    private Text error_message;
    [SerializeField]
    private GameObject ConnectingDisplay;

    void Awake(){
        //DB生成
        string DBPath = Application.persistentDataPath+ "/" + GameUtil.Const.SQLITE_FILE_NAME;
        
        if(!File.Exists(DBPath)){
            Debug.Log("ライブラリ");
            File.Create(DBPath);
        }else{
            Debug.Log("既にDBが存在します。");
            Debug.Log("Path : " + DBPath);
        }
        //SQLiteのテーブル作成
        UserProfile.CreateUserProfileTable();
    }

    void Start(){
        Login();
    }

    //ログイン実行：ユーザ照合されなければ登録画面へ、照合されれば接続画面を経由してメニューシーンへ
    private async void Login(){
        if(UserProfile.Get().name != null){
            //接続中表示
            GUIDisplay.DisplayFadeIn(ConnectingDisplay);

            bool collation = await UserCollation();

            ConnectingDisplay.SetActive(false);
            //サインイン中画面とサインオン画面
            if(collation){
                GUIDisplay.DisplayFadeIn(SignInObj);
                string user_name = UserProfile.Get().name;
                login_text.text = "ユーザ名：" + name + "で接続中です。";

                if(!collation){
                    error_message.text = "アカウント情報が取得できません¥nログインし直すか新しくアカウントを作成してください。";
                }else{
                    //ゲーム画面に移動
                    GUIDisplay.DisplayFadeOut(Canvas);
                    SceneManager.LoadScene("MenuScene");
                }
            }
        }else{
            GUIDisplay.DisplayFadeIn(SignOnObj);
        }
    }

    //サーバーとクライアントのDBでID照合
    private async UniTask<bool> UserCollation(){
        UserProfileModel userProfileSqlite = UserProfile.Get();
        Debug.Log(userProfileSqlite.id + "TEST");
        string userProfileMysqlStr =  await ConnectManager.GetConnectServer("collation","?user_id=" + userProfileSqlite.id);
        UserProfileModel userProfileMysql = ConnectManager.JsonConversion<UserProfileModel>(userProfileMysqlStr);

        //static変数へ格納
        UserParameter.userProfileParameter = userProfileMysql;

        return userProfileSqlite.id == userProfileMysql.id;
    }


    //ユーザ登録ボタン
    public async void OnClickRegister(){
        if(input_name.text == "") return;
        GUIDisplay.DisplayFadeOut(SignOnObj);
        //接続中表示
        GUIDisplay.DisplayFadeIn(ConnectingDisplay);

        //文字数チェック
        if(input_name.text.Length == 0){
            error_message.text = "名前を入力してください";
        }else{
            error_message.text = "";

            try{
                string userProfileMysqlStr = await ConnectManager.GetConnectServer("registration", "?user_name=" + input_name.text);
                
                /*userProfileMysplStrがNullの時の分岐が必要？*/
                UserProfileModel userProfileModel = ConnectManager.JsonConversion<UserProfileModel>(userProfileMysqlStr);
                //Sqliteレコード作成
                UserProfile.Set(userProfileModel);
            }catch(Exception e){
                Debug.Log("エラー");
            }

            Login();
        }
    }
}
