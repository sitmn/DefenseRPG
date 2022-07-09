using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;

public class MenuScreen : MonoBehaviour
{
    //スクリーン入れ替え用クラス
    public static ScreenController screenController;

    //数が変動しないボタンストリーム：ホーム・戻るボタン
    public static IDisposable disposableStaticButtonArr;
    //数が変動するボタンストリーム：ミッション・編成・バトルスクリーンのボタン等
    public static IDisposable disposableDynamicButtonArr;
    //ホーム画面、staticでの参照用
    [SerializeField]
    private CanvasGroup homeScreenRelay;
    public static CanvasGroup homeScreen;

    //スクリーン移動ボタン連打・同時押し防止
    private ReactiveProperty<bool> shareGate = new ReactiveProperty<bool>(true);
    //スクリーン移動ボタン
    [SerializeField]
    private Button[] screenChangeButtonArr;
    [SerializeField]
    private Button[] openWindowButtonArr;
    //スクリーン
    [SerializeField]
    private CanvasGroup[] screenArr;
    [SerializeField]
    private CanvasGroup[] windowArr;

    //所持キャラの情報
    public static List<PossessionCharacterModel> possessionCharacterModelList;
    
    private void Awake(){
        SetStartParameter();
    }

    private void Start(){

    }

    private async void SetStartParameter(){
        string possessionCharacterInfo = await ConnectManager.GetConnectServer("possession_get","?playerId=" + UserParameter.userProfileParameter.id);
        
        possessionCharacterModelList = JsonHelper.FromJson<PossessionCharacterModel>(possessionCharacterInfo);

        homeScreen = homeScreenRelay;
        StartHomeScreen();
    }

    private void StartHomeScreen(){
        screenController = new ScreenController();
        homeScreen.gameObject.SetActive(false);
        screenController.FadeInScreen(homeScreen);

        ChangeScreenButtonSet();
    }

    //スクリーン入れ替え登録
    private void ChangeScreenButtonSet(){
        for(int i=0; i < screenChangeButtonArr.Length ; i ++){
            int n = i;
            screenChangeButtonArr[n].BindToOnClick(shareGate, _ => {
                return ChangeScreenAndButton(screenArr[n]).ToObservable().ForEachAsync(_ => {});
            });
        }
        for(int i=0; i < openWindowButtonArr.Length; i++){
            int n = i;
            openWindowButtonArr[n].BindToOnClick(shareGate, _ => {
                return OpenWindowAndButton(windowArr[n]).ToObservable().ForEachAsync(_ => {});
            });
        }
    }

    private async UniTask ChangeScreenAndButton(CanvasGroup nextScreen){
        IScreen screenScr = nextScreen.gameObject.GetComponent<IScreen>();
        screenScr.CreateButton();
        await screenController.ChangeScreen(nextScreen,homeScreen);
        return;
    }

    private async UniTask OpenWindowAndButton(CanvasGroup nextScreen){
        IScreen screenScr = nextScreen.gameObject.GetComponent<IScreen>();
        screenScr.CreateButton();
        await screenController.OpenWindow(nextScreen);
        return;
    }


    /*public void CreateButtonStream(List<Button> buttonArr){
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
                JudgeScreen(x);})
            .AddTo(this);
        }else{
            disposableDynamicButtonArr = Observable.Merge(buttonStreamArr)
            .Subscribe(x => {
                JudgeScreen(x);})
            .AddTo(this);
        }
    }*/

    /*//各スクリーンへ処理を割り当て
    private void JudgeScreen(Button button){
        ButtonBase buttonBase = button.GetComponent<ButtonBase>();
        IMenuProvider menuProvider = buttonBase.menuProvider;
        menuProvider.JudgeButton(buttonBase.ButtonNo);
    }

    //ボタン同時押し、連打防止用フラグセット
    private async void ButtonFlagSet(ButtonBase buttonBase){
        if(buttonBase.ButtonNo == 0){
            ScreenChangeButton screenChangeButton = buttonBase as ScreenChangeButton;
            if(screenChangeButton.NextScreen != activeScreen) return;
        }
        
        buttonStreamFlag = false;
        await UniTask.Delay((int)buttonBase.DelayTime);
        buttonStreamFlag = true;
    }*/
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
