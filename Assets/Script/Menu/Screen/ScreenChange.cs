using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

public class ScreenChange : MonoBehaviour,IScreenProvider
{
    //スクリーン入れ替え：フェードアウト
    public async UniTask FadeOutScreen(ScreenChangeButton buttonInfoScript){
        await MenuScript.activeScreen.DOFade(0.0f, buttonInfoScript.DelayTime/(1000f*2f));
        MenuScript.activeScreen.gameObject.SetActive(false);
        Debug.Log("1");
        return;
    }

    //スクリーン入れ替え：フェードイン
    public async UniTask FadeInScreen(ScreenChangeButton buttonInfoScript){
        Debug.Log("3");
        buttonInfoScript.NextScreen.gameObject.SetActive(true);
        //ScreenButtonSet(buttonInfoScript.NextScreen.gameObject);
        await buttonInfoScript.NextScreen.DOFade(1.0f, buttonInfoScript.DelayTime/(1000f*2f));

        MenuScript.activeScreen = buttonInfoScript.NextScreen;
    
        return;
    }

    //ウィンドウを開く
    public async UniTask OpenWindow(ScreenWindowButton buttonInfoScript){
        buttonInfoScript.NextScreen.gameObject.SetActive(true);
        await buttonInfoScript.NextScreen.gameObject.GetComponent<Transform>().DOScale(new Vector3(1.0f,1.0f,1.0f),buttonInfoScript.DelayTime/1000f);
        return;
    }

    //ウィンドウを閉じる
    public async UniTask CloseWindow(ScreenWindowButton buttonInfoScript){
        await buttonInfoScript.NextScreen.gameObject.GetComponent<Transform>().DOScale(new Vector3(1.0f,0f,1.0f),buttonInfoScript.DelayTime/1000f);
        buttonInfoScript.NextScreen.gameObject.SetActive(false);
        return;
    }
}
