using UnityEngine;
using UniRx;

//プレイヤー操作用インターフェース
public interface IPlayerProvider
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
