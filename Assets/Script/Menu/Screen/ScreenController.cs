using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

public class ScreenController : MonoBehaviour
{
    private float fadeTime = 1000f;
    private float windowTime = 500f;
    
    //スクリーン入れ替え：フェードアウト
    public async UniTask FadeOutScreen(CanvasGroup activeScreen){
        await activeScreen.DOFade(0.0f, fadeTime/(1000f*2f));
        activeScreen.gameObject.SetActive(false);
        return;
    }

    //スクリーン入れ替え：フェードイン
    public async UniTask FadeInScreen(CanvasGroup nextScreen){
        nextScreen.gameObject.SetActive(true);
        //ScreenButtonSet(buttonInfoScript.NextScreen.gameObject);
        await nextScreen.DOFade(1.0f, fadeTime/(1000f*2f));
        return;
    }

    //ウィンドウを開く
    public async UniTask OpenWindow(CanvasGroup openScreen){
        openScreen.gameObject.SetActive(true);
        await openScreen.gameObject.GetComponent<Transform>().DOScale(new Vector3(1.0f,1.0f,1.0f),windowTime/1000f);
        return;
    }

    //ウィンドウを閉じる
    public async UniTask CloseWindow(CanvasGroup closeScreen){
        await closeScreen.gameObject.GetComponent<Transform>().DOScale(new Vector3(1.0f,0f,1.0f),windowTime/1000f);
        closeScreen.gameObject.SetActive(false);
        return;
    }

    //スクリーン入れ替え
    public async UniTask ChangeScreen(CanvasGroup nextScreen,CanvasGroup activeScreen){
        await FadeOutScreen(activeScreen);
        await FadeInScreen(nextScreen);
        return;
    }
}
