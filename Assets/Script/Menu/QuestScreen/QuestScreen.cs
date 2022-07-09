using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;

public class QuestScreen : MonoBehaviour,IScreen
{
    //ホームへ戻るボタン
    [SerializeField]
    private Button backButton;

    //スクリーン入れ替え用
    //ScreenController screenController;
    /*****クエストスクリーン*****/
    //クエストスクリーンのボタン配列
    [SerializeField]
    private List<Button> questButtonArr;
    /*************************/

    void Awake(){
        BackButtonSet();
    }

    public void BackButtonSet(){
        ScreenController screenController = new ScreenController();
        CanvasGroup questScreen = gameObject.GetComponent<CanvasGroup>();
        backButton.BindToOnClick(_ =>{
            return screenController.ChangeScreen(MenuScreen.homeScreen, questScreen).ToObservable().ForEachAsync(_ => {});
        });
    }

    public void CreateButton(){

    }
}
