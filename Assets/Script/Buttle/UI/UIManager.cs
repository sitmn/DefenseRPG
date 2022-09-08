using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //アクション実行用ゲージ
    public ActionGauge _actionGauge;
    //リフトアップボタン
    public LiftUpButton _liftUpButton;
    //リフトダウンボタン
    public LiftDownButton _liftDownButton;
    [SerializeField]
    private Transform _playerTr;

    //クラスの初期化
    public void AwakeManager(){
        _actionGauge.AwakeManager(_playerTr);
    }

}
