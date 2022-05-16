using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using DG.Tweening;

public class MenuScript : MonoBehaviour,IMenuProvider
{
    //スクリーン入れ替え用インターフェース
    [SerializeReference]
    private GameObject ScreenProviderObj;
    private IScreenProvider screenProvider;

    //数が変動しないボタンストリーム：ホーム・戻るボタン
    public static IDisposable disposableStaticButtonArr;
    //数が変動するボタンストリーム：ミッション・編成・バトルスクリーンのボタン等
    public static IDisposable disposableDynamicButtonArr;
    //今有効な画面、staticでの参照用
    [SerializeField]
    public CanvasGroup activeScreenRelay;
    public static CanvasGroup activeScreen;

    //スクリーン移動ボタン配列
    [SerializeField]
    private List<Button> screenChangeButtonArr;
    /*****クエストスクリーン*****/
    //クエストスクリーンのボタン配列
    [SerializeField]
    private List<Button> questButtonArr;
    /*************************/

    /******対戦スクリーン*******/
    //対戦スクリーンのボタン配列
    [SerializeField]
    private List<Button> buttleButtonArr;
    /*************************/
    
    /*******編成スクリーン*******/
    //編成スクリーンのボタン配列
    private List<Button> teamButtonArr;
    [SerializeField]
    private GameObject teamFrame;
    [SerializeField]
    private GameObject charFrame;
    //所持キャラクター
    public List<PossessionCharacterModel> possessionCharacterModelList;
    /**************************/
    
    /*******強化スクリーン*******/
    //強化スクリーンのボタン配列
    [SerializeField]
    private List<Button> strengthenButtonArr;
    /**************************/

    /*******ガチャスクリーン******/
    //ガチャスクリーンのボタン配列
    [SerializeField]
    private List<Button> gachaButtonArr;
    /**************************/

    /*****ミッションスクリーン*****/
    //ミッションスクリーンのボタン配列
    [SerializeField]
    private List<Button> missionButtonArr;
    /***************************/
    
    /*****プレゼントスクリーン*****/
    //プレゼントスクリーンのボタン配列
    [SerializeField]
    private List<Button> presentButtonArr;
    /***************************/


    [SerializeField]
    private Button buttonPrefab;


    //ボタンのストリームフラグ
    public static bool buttonStreamFlag = true;

    private void Awake(){
        SetStartParameter();
    }

    private void Start(){
        CreateButtonStream(screenChangeButtonArr);
    }

    private async void SetStartParameter(){
        string possessionCharacterInfo = await ConnectManager.GetConnectServer("possession_get","?playerId=" + UserParameter.userProfileParameter.id);
        Debug.Log(possessionCharacterInfo+"AAA");
        
        possessionCharacterModelList = JsonHelper.FromJson<PossessionCharacterModel>(possessionCharacterInfo);
        //possessionCharacterModelList = new List<PossessionCharacterModel>(pArray);
        foreach(PossessionCharacterModel a in possessionCharacterModelList){
            Debug.Log(a.id);
        }
        //interfaceをinspectorから参照できないためObjから参照
        screenProvider = ScreenProviderObj.GetComponent<ScreenChange>();
        teamButtonArr = new List<Button>();

        activeScreen = activeScreenRelay;
        activeScreen.gameObject.SetActive(false);
        GUIDisplay.DisplayFadeIn(activeScreen.gameObject);
    }

    public void CreateButtonStream(List<Button> buttonArr){
        IObservable<Button>[] buttonStreamArr = new IObservable<Button>[buttonArr.Count];
        for(int i = 0; i < buttonArr.Count ; i ++){
            int n = i;
            buttonStreamArr[n] = buttonArr[n].OnClickAsObservable()
                .TakeUntilDestroy(buttonArr[n].gameObject)
                .Select(_ => buttonArr[n])
                .Where(_ => buttonStreamFlag);
        }
        
        //動的・静的なボタン群のストリームを作成
        if(buttonArr == screenChangeButtonArr){
        disposableStaticButtonArr = Observable.Merge(buttonStreamArr)
            .Subscribe(x => {
                JudgeButton(x);})
            .AddTo(this);
        }else{
            disposableDynamicButtonArr = Observable.Merge(buttonStreamArr)
            .Subscribe(x => {
                JudgeButton(x);})
            .AddTo(this);
        }
    }

    //ボタン配列の格納順からボタンの内容を判別
    /*
    0:スクリーン入れ替え
    1:ウィンドウを開く
    2:ウィンドウを閉じる
    3:
    4:
    */
    private async void JudgeButton(Button button){
        ButtonBase buttonInfoScript = button.gameObject.GetComponent<ButtonBase>();
        ButtonFlagSet(buttonInfoScript);
        if(buttonInfoScript.ButtonNo == 0){
            ScreenChangeButton screenChangeButton = buttonInfoScript as ScreenChangeButton;
            
            if(screenChangeButton.NextScreen.name == "TeamScreen"){
                CreateButton(charFrame, teamButtonArr);
            }

            await screenProvider.FadeOutScreen(screenChangeButton);
            await screenProvider.FadeInScreen(screenChangeButton);
        } 
        else if(buttonInfoScript.ButtonNo == 1){
            ScreenWindowButton screenWindowButton = buttonInfoScript as ScreenWindowButton;
            screenProvider.OpenWindow(screenWindowButton);
        }
        else if(buttonInfoScript.ButtonNo == 2){
            ScreenWindowButton screenWindowButton = buttonInfoScript as ScreenWindowButton;
            screenProvider.CloseWindow(screenWindowButton);
        } 
        else if(buttonInfoScript.ButtonNo == 3){

        }

    }

    private async void ButtonFlagSet(ButtonBase buttonBase){
        if(buttonBase.ButtonNo == 0){
            ScreenChangeButton screenChangeButton = buttonBase as ScreenChangeButton;
            if(screenChangeButton.NextScreen != activeScreen) return;
        }
        
        buttonStreamFlag = false;
        await UniTask.Delay((int)buttonBase.DelayTime);
        buttonStreamFlag = true;
    }

    //スクリーン内ボタン生成
    public async void CreateButton(GameObject buttonParent, List<Button> buttonList){
        if(disposableDynamicButtonArr != null) disposableDynamicButtonArr.Dispose();
        foreach(Button button in buttonList){
            Destroy(button.gameObject);
        }
        InstantiateButton(buttonParent,buttonList);
        //CreateButtonStream(buttonList);
    }

    private void InstantiateButton(GameObject buttonParent,List<Button> buttonList){
        //キャラマスターテーブルからキャラを全出力
        buttonList = new List<Button>();

        for(int i = 0; i < possessionCharacterModelList.Count; i++){
            Button button = Instantiate(buttonPrefab, buttonParent.transform.position + new Vector3(200 * (i % 5) - 450, -200 * ((i / 5) - 1),0),Quaternion.identity);
            buttonList.Add(button);
            button.gameObject.transform.parent = buttonParent.transform;
            //ボタンにキャラ情報を表示させてFlagをオンにして、チームメンバに表示させるようなものを入れる
        
        }


    }
}

[Serializable]
public class PossessionCharacterModel{
    public int id;
    public int playerId;
    public int characterId;
    public int level;
    public int team_flag;
    public DateTime created_at;
    public DateTime updated_at;
}

[Serializable]
public class CharacterMasterModel{
    public int id;
    public string name;
    public int base_HP;
    public int base_attack;
    public int base_defense;
    public float base_speed;
    public float attack_speed;
    public float attack_range;
    public int character_cost;
    public int attack_type;
    public float HP_magnification;
    public float attack_magnification;
    public float defense_magnification;
    public float speed_magnification;
    public float attack_speed_magnification;
    public DateTime created_at;
    public DateTime updated_at;
}
