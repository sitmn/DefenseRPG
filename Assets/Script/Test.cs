using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    //レコード取得ボタン：テスト
    public void OnClickSelect(){
        UserProfileModel userProfileModel = UserProfile.Get();
        Debug.Log("ユーザ名：" + userProfileModel.user_name);
    }

    //リセットボタン：テスト
    public void OnClickReset(){
        UserProfile.Delete();
        Debug.Log("テーブルを削除しました。");
    }

    //ボタンデバッグ
    public void OnClickButton(){
        Debug.Log("ボタン");
    }
}
