using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;

public class TeamScreen : MonoBehaviour, IScreen
{
    [SerializeField]
    private Button backButton;
    [SerializeField]
    private GameObject teamFrame;
    [SerializeField]
    private GameObject charFrame;
    [SerializeField]
    private Button buttonPrefab;
    private List<Button> charList;

    void Awake(){
        BackButtonSet();
    }

    //ホームスクリーン切り替え
    public void BackButtonSet(){
        ScreenController screenController = new ScreenController();
        CanvasGroup teamScreen = gameObject.GetComponent<CanvasGroup>();
        backButton.BindToOnClick(_ =>{
            return screenController.ChangeScreen(MenuScreen.homeScreen, teamScreen).ToObservable().ForEachAsync(_ => {});
        });
    }

    //スクリーン内ボタン生成
    public async void CreateButton(){
        if(charList == null) charList = new List<Button>();
        
        foreach(Button button in charList){
            Destroy(button.gameObject);
        }
        InstantiateButton();
    }

    private void InstantiateButton(){
        //キャラマスターテーブルからキャラを全出力
        charList = new List<Button>();

        for(int i = 0; i < MenuScreen.possessionCharacterModelList.Count; i++){
            Button button = Instantiate(buttonPrefab, charFrame.transform.position + new Vector3(200 * (i % 5) - 450, -200 * ((i / 5) - 1),0),Quaternion.identity);
            charList.Add(button);
            button.gameObject.transform.parent = charFrame.transform;
            //ボタンにキャラ情報を表示させてFlagをオンにして、チームメンバに表示させるようなものを入れる
        
        }


    }

}
