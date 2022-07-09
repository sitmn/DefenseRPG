using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;

public class GachaScreen : MonoBehaviour,IScreen
{
    [SerializeField]
    private Button backButton;

    /*******ガチャスクリーン******/
    //ガチャスクリーンのボタン配列
    [SerializeField]
    private List<Button> gachaButtonArr;
    /**************************/

    void Awake(){
        BackButtonSet();
    }

    //ホームスクリーン切り替え
    public void BackButtonSet(){
        ScreenController screenController = new ScreenController();
        CanvasGroup gachaScreen = gameObject.GetComponent<CanvasGroup>();
        backButton.BindToOnClick(_ =>{
            return screenController.ChangeScreen(MenuScreen.homeScreen, gachaScreen).ToObservable().ForEachAsync(_ => {});
        });
    }

    public void CreateButton(){
        
    }

}
