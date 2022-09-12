using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    //アクション実行用ゲージ
    public ActionGauge _actionGauge;
    //コストゲージ
    public CostGauge _costGauge;
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
    //修復ボタン
    public RepairButton _repairButton;
    
    [SerializeField]
    private Transform _playerTr;
    [SerializeField]
    private PlayerInput _playerInput;

    //クラスの初期化
    public void AwakeManager(SystemParam _systemParam){
        _actionGauge.AwakeManager(_playerTr);
        _costGauge.AwakeManager(_systemParam);
        _selectLaunchButtonList.AwakeManager(_playerInput);
        _liftUpButton.AwakeManager();
        _liftDownButton.AwakeManager();
        _launchButton.AwakeManager();
        _rankUpButton.AwakeManager();
        _repairButton.AwakeManager();
    }

}
