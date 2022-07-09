using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;

public class ButtleScreen : MonoBehaviour, IScreen
{
    //スクリーン入れ替え用
    [SerializeField]
    private Button backButton;
    /******対戦スクリーン*******/
    //対戦スクリーンのボタン配列
    [SerializeField]
    private List<Button> buttleButtonArr;
    /*************************/

    void Awake(){
        BackButtonSet();
    }

    //ホームスクリーン切り替え
    public void BackButtonSet(){
        ScreenController screenController = new ScreenController();
        CanvasGroup buttleScreen = gameObject.GetComponent<CanvasGroup>();
        backButton.BindToOnClick(_ =>{
            return screenController.ChangeScreen(MenuScreen.homeScreen, buttleScreen).ToObservable().ForEachAsync(_ => {});
        });
    }

    public void CreateButton(){
        
    }

}
