using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;

public class PresentScreen : MonoBehaviour,IScreen
{
    [SerializeField]
    private Button[] backButton;
    /*****プレゼントスクリーン*****/
    //プレゼントスクリーンのボタン配列
    [SerializeField]
    private List<Button> presentButtonArr;
    /***************************/

    void Awake(){
        BackButtonSet();
    }

    //ホームスクリーン切り替え
    public void BackButtonSet(){
        ScreenController screenController = new ScreenController();
        CanvasGroup presentScreen = gameObject.GetComponent<CanvasGroup>();
        for(int i =0 ; i < backButton.Length; i++){
                int n = i;
                backButton[n].BindToOnClick(_ =>{
                return screenController.CloseWindow(presentScreen).ToObservable().ForEachAsync(_ => {});
            });
        }
        
    }

    public void CreateButton(){
        
    }

}
