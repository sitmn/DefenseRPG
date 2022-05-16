using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

interface IScreenProvider
{
    //スクリーンをフェードアウトで非表示
    UniTask FadeOutScreen(ScreenChangeButton buttonInfoScript);
    //スクリーンをフェードインで表示
    UniTask FadeInScreen(ScreenChangeButton buttonInfoScript);
    //スクリーンを開く
    UniTask OpenWindow(ScreenWindowButton buttonInfoScript);
    //スクリーンを閉じる
    UniTask CloseWindow(ScreenWindowButton buttonInfoScript);
}
