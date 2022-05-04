using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using DG.Tweening;

public class MenuScript : MonoBehaviour
{
    //数が変動しないボタンストリーム：ホーム・戻るボタン
    IDisposable disposableStaticButtonArr;
    //数が変動するボタンストリーム：ミッション・編成・バトルスクリーンのボタン等
    IDisposable disposableDynamicButtonArr;
    //今有効な画面
    [SerializeField]
    private CanvasGroup activeScreen;

    //ホームスクリーンのボタン配列
    [SerializeField]
    private List<Button> staticButtonArr;
    //クエストスクリーンのボタン配列
    [SerializeField]
    private List<Button> questButtonArr;
    //対戦スクリーンのボタン配列
    [SerializeField]
    private List<Button> buttleButtonArr;
    //編成スクリーンのボタン配列
    [SerializeField]
    private List<Button> teamButtonArr;
    //強化スクリーンのボタン配列
    [SerializeField]
    private List<Button> strengthenButtonArr;
    //ガチャスクリーンのボタン配列
    [SerializeField]
    private List<Button> gachaButtonArr;
    //ミッションスクリーンのボタン配列
    [SerializeField]
    private List<Button> missionButtonArr;
    //プレゼントスクリーンのボタン配列
    [SerializeField]
    private List<Button> presentButtonArr;
    //ボタンストリームのフラグ
    [SerializeField]
    private bool buttonStreamFlag = true;

    //編成スクリプト
    [SerializeField]
    private TeamScript teamScript;

    private void Awake(){
        activeScreen.gameObject.SetActive(false);
        GUIDisplay.DisplayFadeIn(activeScreen.gameObject);
    }

    private void Start(){
        ButtonStreamCreate(staticButtonArr);
    }

    public void ButtonStreamCreate(List<Button> buttonArr){
        IObservable<Button>[] buttonStreamArr = new IObservable<Button>[buttonArr.Count];
        for(int i = 0; i < buttonArr.Count ; i ++){
            int n = i;
            buttonStreamArr[n] = buttonArr[n].OnClickAsObservable()
                .TakeUntilDestroy(buttonArr[n].gameObject)
                .Select(_ => buttonArr[n])
                .Where(_ => buttonStreamFlag);
        }
        
        //動的・静的なボタン群のストリームを作成
        if(buttonArr == staticButtonArr){
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
    private void JudgeButton(Button button){
        ButtonInfoScript buttonInfoScript = button.gameObject.GetComponent<ButtonInfoScript>();
        if(buttonInfoScript.NextScreen != activeScreen) ButtonFlagSet(buttonInfoScript.ButtonDelayTime);
        if(buttonInfoScript.ButtonNo == 0) ScreenChange(buttonInfoScript);
        else if(buttonInfoScript.ButtonNo == 1) OpenWindow(buttonInfoScript);
        else if(buttonInfoScript.ButtonNo == 2) CloseWindow(buttonInfoScript);
    }

    //スクリーン入れ替え：フェードイン、アウト
    private async void ScreenChange(ButtonInfoScript buttonInfoScript){
        await activeScreen.DOFade(0.0f, buttonInfoScript.ButtonDelayTime/(1000f*2f));
        activeScreen.gameObject.SetActive(false);
        buttonInfoScript.NextScreen.gameObject.SetActive(true);
        ScreenButtonSet(buttonInfoScript.NextScreen.gameObject);
        await buttonInfoScript.NextScreen.DOFade(1.0f, buttonInfoScript.ButtonDelayTime/(1000f*2f));

        activeScreen = buttonInfoScript.NextScreen;

        return;
    }

    //ウィンドウを開く
    private async void OpenWindow(ButtonInfoScript buttonInfoScript){
        buttonInfoScript.NextScreen.gameObject.SetActive(true);
        await buttonInfoScript.NextScreen.gameObject.GetComponent<Transform>().DOScale(new Vector3(1.0f,1.0f,1.0f),buttonInfoScript.ButtonDelayTime/1000f);
        return;
    }

    //ウィンドウを閉じる
    private async void CloseWindow(ButtonInfoScript buttonInfoScript){
        await buttonInfoScript.NextScreen.gameObject.GetComponent<Transform>().DOScale(new Vector3(1.0f,0f,1.0f),buttonInfoScript.ButtonDelayTime/1000f);
        buttonInfoScript.NextScreen.gameObject.SetActive(false);
        return;
    }

    private async void ButtonFlagSet(float delayTime){
        buttonStreamFlag = false;
        await UniTask.Delay((int)delayTime);
        buttonStreamFlag = true;
    }

    //スクリーン内ボタン生成
    private async void ScreenButtonSet(GameObject screenGameObject){
        //disposableDynamicButtonArr.Dispose();
        if(screenGameObject.name == "TeamScreen"){
            teamScript.InstantiateButton();
        }
    }
}
