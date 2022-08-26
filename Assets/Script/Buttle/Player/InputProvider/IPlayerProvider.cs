using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

//プレイヤー操作用インターフェース
interface IPlayerProvider
{
    //スティック移動
    IReadOnlyReactiveProperty<Vector2> MoveDir{get;}
    //クリスタル起動ボタン
    IReadOnlyReactiveProperty<float> Launch {get;}
    
    //スキル1ボタン
    //スキル2ボタン
    //カメラ回転ボタン
    //ポーズボタン
    //キャラ変更ボタン
}
