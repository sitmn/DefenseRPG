using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    //アクション実行用ゲージ
    public ActionGauge _actionGauge;
    //リフトアップボタン
    public LiftUpButton _liftUpButton;
    //リフトダウンボタン
    public LiftDownButton _liftDownButton;
    //クリスタルセレクトボタン
    public SelectLaunchButtonList _selectLaunchButtonList;
    //クリスタル起動ボタン
    public LaunchButton _launchButton;
    //クリスタルランクアップボタン
    public RankUpButton _rankUpButton;
    [SerializeField]
    private Transform _playerTr;
    [SerializeField]
    private PlayerInput _playerInput;

    //クラスの初期化
    public void AwakeManager(){
        _actionGauge.AwakeManager(_playerTr);
        _selectLaunchButtonList.AwakeManager(_playerInput);
    }

}
