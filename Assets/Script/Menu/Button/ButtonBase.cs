using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBase : MonoBehaviour
{
        /*buttonNo:ボタン判別用番号
    0:スクリーン変更:nextScreenのスクリーンに変更
    1:スクリーンOpen
    2:スクリーンClose
    3:キャラ選択:キャラ編成ボタン、解除ボタン、キャラ情報するコンポーネントを表示
    4:キャラ編成:編成キャラ入れ替え画面に変更、キャラ入れ替えボタンが表示　or　編成空きがあれば確認なしでセット
    5:キャラ編成解除:編成キャラから削除
    6:キャラ入れ替えボタン:編成予定キャラと編成中キャラを入れ替え
    7:ミッション完了ボタン:
    8:キャラ強化ボタン:
    9:
    */
    [SerializeField]
    private int buttonNo;
    public int ButtonNo{
        get{return buttonNo;}
    }

    [SerializeField]
    private float delayTime;
    public float DelayTime{
        get{return delayTime;}
    }
}
