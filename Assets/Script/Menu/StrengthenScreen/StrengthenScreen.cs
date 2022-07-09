using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;

public class StrengthenScreen : MonoBehaviour,IScreen
{
    [SerializeField]
    private Button backButton;

    /*******強化スクリーン*******/
    //強化スクリーンのボタン配列
    [SerializeField]
    private List<Button> strengthenButtonArr;
    /**************************/

    void Awake(){
        BackButtonSet();
    }

    //ホームスクリーン切り替え
    public void BackButtonSet(){
        ScreenController screenController = new ScreenController();
        CanvasGroup strengthenScreen = gameObject.GetComponent<CanvasGroup>();
        backButton.BindToOnClick(_ =>{
            return screenController.ChangeScreen(MenuScreen.homeScreen, strengthenScreen).ToObservable().ForEachAsync(_ => {});
        });
    }

    public void CreateButton(){

    }
}
