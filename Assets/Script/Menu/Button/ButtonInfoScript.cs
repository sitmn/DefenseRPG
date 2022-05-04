using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInfoScript : MonoBehaviour
{
    //ボタン連打防止用の時間
    [SerializeField]
    private int buttonDelayTime;
    public int ButtonDelayTime{
        get{return buttonDelayTime;}
    }

    /*buttonNo:ボタン判別用番号
    0:スクリーン変更:nextScreenのスクリーンに変更
    1:キャラ選択:キャラ編成ボタン、解除ボタン、キャラ情報するコンポーネントを表示
    2:キャラ編成:編成キャラ入れ替え画面に変更、キャラ入れ替えボタンが表示　or　編成空きがあれば確認なしでセット
    3:キャラ編成解除:編成キャラから削除
    4:キャラ入れ替えボタン:編成予定キャラと編成中キャラを入れ替え
    5:ミッション完了ボタン:
    6:キャラ強化ボタン:
    7:
    */
    [SerializeField]
    private int buttonNo;
    public int ButtonNo{
        get{return buttonNo;}
    }

    //buttonNo:0で使用
    [SerializeField]
    private CanvasGroup nextScreen;
    public CanvasGroup NextScreen{
        get{return nextScreen;}
    }

    //buttonNo:1で使用　所持キャラリストの判別で使用
    private int characterPossessionNo;
    public int CharacterPossession{
        get{return characterPossessionNo;}
        set{characterPossessionNo = value;}
    }

    //buttonNo:1で使用　編成中キャラリストの判別で使用
    private int characterOrgNo;
    public int CharacterOrgNo{
        get{return characterOrgNo;}
        set{characterOrgNo = value;}
    }
}
