using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Linq;
//using System.Threading;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

    [System.Serializable]
    public class ResponseObjects
    {
	    public UserProfileModel user_profile;
    }

public class ConnectManager : MonoBehaviour
{
    public static async UniTask<string> ConnectServer(string endpoint, string parameter){
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(GameUtil.Const.SERVER_URL + endpoint + parameter);
        await unityWebRequest.SendWebRequest();

        if(!string.IsNullOrEmpty(unityWebRequest.error)){
            Debug.LogError(unityWebRequest.error);
            return unityWebRequest.downloadHandler.text;
        }

        string text = unityWebRequest.downloadHandler.text;
        Debug.Log("レスポンス：" + text);

        if(text.All(char.IsNumber)){
            switch(text){
                case GameUtil.Const.ERROR_DB_UPDATE:
					Debug.LogError("サーバーでエラーが発生しました。[データベース更新エラー]");
					break;
				default:
					Debug.LogError("サーバーでエラーが発生しました。[システムエラー]");
					break;
            }
            return text;
        }

    return text;
    }

    public static UserProfileModel JsonConversion(string jsonText){
        UserProfileModel conversionClass = JsonUtility.FromJson<UserProfileModel>(jsonText);
        return conversionClass;
    }
}
